using Application.Features.Management.Products.Dtos;
using Domain.Brands;
using Domain.Categories;
using Domain.Common.ValueObjects;
using Domain.Inventories;
using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;
using Domain.Warehouses;

namespace Application.Features.Management.Products.Commands.CreateProduct;

internal sealed class CreateProductCommandHandler(IAppDbContext db,
                                                  IImageValidator validator,
                                                  IImageStorageService imageStore,
                                                  IIdGenerator<ProductGroupId> groupIdGen,
                                                  IIdGenerator<ProductId> productIdGen,
                                                  IIdGenerator<long> IImageGen) : IRequestHandler<CreateProductCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreateProductCommand request, CancellationToken ct)
    {
        BrandId brandId = new(request.BrandId);
        CategoryId categoryId = new(request.CategoryId);

        var brandName = await db.Brands.AsNoTracking()
                                        .Where(x => x.Id == brandId)
                                        .Select(x => x.Name)
                                        .FirstOrDefaultAsync(ct);

        var categoryName = await db.Categories.AsNoTracking()
                                        .Where(x => x.Id == categoryId)
                                        .Select(x => x.Name)
                                        .FirstOrDefaultAsync(ct);

        if (brandName is null || categoryName is null)
            return brandName is null ? ApplicationErrors.NotFound.Brand : ApplicationErrors.NotFound.Category;

        ProductGroupId productGroupId = groupIdGen.NewId();

        var createProductResult = ProductGroup.Create(
            productGroupId,
            brandId,
            brandName,
            categoryId,
            categoryName,
            request.Title,
            request.Description,
            request.IsSerialized,
            request.Specifications
        );

        if (createProductResult.Failed)
        {
            return createProductResult.Errors;
        }
        var productGroup = createProductResult.Value;

        db.ProductGroups.Add(productGroup);
        await db.SaveAsync(ct);//saving now to prevent circuluar dependency error

        var productId = productIdGen.NewId();
        var price = Money.Create(request.Price);


        var addProductResult = productGroup.AddProduct(productId, price,
            request.Width, request.Height, request.Length, request.Weight,
            request.Sku, request.Slug, request.BarCode);

        if (addProductResult.Failed)
        {
            await Rollback(productGroupId, ct);
            return addProductResult.Errors;
        }


        var createInventoryResult = CreateProductInventories(request.Inventory, productId);

        if (createInventoryResult.Failed)
        {
            await Rollback(productGroupId, ct);

            return createInventoryResult.Errors;
        }
        db.Inventories.Add(createInventoryResult.Value);

        if (request.UploadedImages != null && request.UploadedImages.Count != 0)
        {
            var result = await AddImages(productGroup, productId, request.UploadedImages, ct); // ensure to be the last execution so the server doesn't save files unnecasserly

            if(result.Failed)
            {
                await Rollback(productGroupId, ct);
                return result.Errors;
            }
        }


        await db.SaveAsync(ct);

        return productId.Value;
    }

    internal static Result<Inventory> CreateProductInventories(InventoryDto? inventoryDto, ProductId productId)
    {
        int stockQuantity = 0;
        if(inventoryDto is not null)
        {
            stockQuantity = inventoryDto.StockQuantity;
        }
        var result = Inventory.Create(new WarehouseId(20), productId, stockQuantity);

        return result;
    }

    private async Task<Result<Success>> AddImages(ProductGroup productGroup, ProductId productId, List<FileUploadDto> files, CancellationToken ct)
    {
        validator.MinWidth = ApplicationRules.Uploads.MinWidth;
        validator.MinHeight = ApplicationRules.Uploads.MinHeight;
        validator.MaxSize = ApplicationRules.Uploads.MaxProductImageSize; ;

        var filesValdationResult = validator.ValidateAll(files);

        if (filesValdationResult.Failed)
            return filesValdationResult.Errors;

        var images = files.Select((x, index) =>
                        ProductImage.Create(IImageGen.NewId(), x.InternalFileName, (byte)index)).ToList();

        var addImagesResult = productGroup.UpdateImages(productId, images);

        if (addImagesResult.Failed)
            return addImagesResult.Errors;


        var result = await imageStore.SaveAllAsync(files, ct);

        if (result.Failed)
            return result.Errors;

        return Result.Success;
    }

    private async Task Rollback(ProductGroupId id, CancellationToken ct)
    {
        await db.ProductGroups.Where(pg => pg.Id == id).ExecuteDeleteAsync(ct);

    }

}
