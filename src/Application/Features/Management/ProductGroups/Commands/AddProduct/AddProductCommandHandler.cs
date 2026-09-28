
using Application.Features.Management.ProductGroups.Dtos;
using Domain.Common.ValueObjects;
using Domain.Inventories;
using Domain.ProductGroups.Products;

namespace Application.Features.Management.ProductGroups.Commands.AddProduct;

public sealed class AddProductCommandHandler(IAppDbContext context, IIdGenerator<ProductId> idGen, IImageValidator validator, IImageStorageService imageStore) : IRequestHandler<AddProductCommand, Result<long>>
{
    public async Task<Result<long>> Handle(AddProductCommand request, CancellationToken ct)
    {

        ProductGroupId productGroupId = new(request.ProductGroupId);

        var price = Money.Create((decimal)request.Price);

        var productGroup = await context.ProductGroups.FirstOrDefaultAsync(x => x.Id == productGroupId, ct);

        if (productGroup is null)
        {
            return ApplicationErrors.NotFound.Product;
        }

        var productId = idGen.NewId();

        var addProductResult = productGroup.AddProduct(productId, price,
            request.Width, request.Height, request.Length, request.Weight,
            request.Sku, request.Slug, request.BarCode, request.Specifications);

        if (addProductResult.Failed)
        {
            return addProductResult.Errors;
        }

        if (request.Images != null && request.Images.Count != 0)
        {
            await AddImages(productGroup, productId, request.Images, ct);
        }

        if (request.StockPerWarehouse != null && request.StockPerWarehouse.Count != 0)
        {
            var result = CreateProductInventories(request.StockPerWarehouse, productId);

            if (result.Failed)
                return result.Errors;

            context.Inventories.AddRange(result.Value);
        }

        await context.SaveAsync(ct);

        return productId.Value;
    }

    internal static Result<List<Inventory>> CreateProductInventories(List<ProductStockDto> StockPerWarehouse, ProductId productId)
    {

        List<Inventory> inventories = new(StockPerWarehouse.Count);

        foreach (var stock in StockPerWarehouse)
        {
            var result = Inventory.Create(stock.ParsedWarehouseId, productId, stock.StockQuantity);

            if (result.Failed)
                return result.Errors;

            inventories.Add(result.Value);
        }
        return inventories;
    }

    private async Task<Result<Success>> AddImages(ProductGroup productGroup, ProductId productId, List<FileUploadDto> images, CancellationToken ct)
    {
        validator.MinWidth = ApplicationRules.Uploads.MinWidth;
        validator.MinHeight = ApplicationRules.Uploads.MinHeight;
        validator.MaxSize = ApplicationRules.Uploads.MaxProductImageSize; ;

        var imagesValdationResult = validator.ValidateAll(images);

        if (imagesValdationResult.Failed)
            return imagesValdationResult.Errors;

        List<string> imageNames = new(images.Count);

        images.ForEach(image =>
        {
            imageNames.Add(image.InternalFileName);
        });

        var addImagesResult = productGroup.AddProductImages(productId, imageNames);

        if (addImagesResult.Failed)
            return addImagesResult.Errors;


        var result = await imageStore.SaveAllAsync(images, ct);

        if (result.Failed)
            return result.Errors;

        return Result.Success;
    }


}
