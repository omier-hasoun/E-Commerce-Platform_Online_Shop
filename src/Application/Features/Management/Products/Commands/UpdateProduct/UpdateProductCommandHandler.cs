using Domain.Brands;
using Domain.Categories;
using Domain.Common.ValueObjects;
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.UpdateProduct;

internal sealed class UpdateProductCommandHandler(IAppDbContext db) : IRequestHandler<UpdateProductCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateProductCommand command, CancellationToken ct)
    {

        ProductId productId = new(command.ProductId);

        var productGroup = await db.ProductGroups.Include(x => x.MainProduct)
                                                      .Where(x => x.MainProduct!.Id == productId)
                                                      .FirstOrDefaultAsync(ct);

        if (productGroup is null)
        {
            return ApplicationErrors.NotFound.Product;
        }


        BrandId brandId = new(command.BrandId);

        CategoryId categoryId = new(command.CategoryId);
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

        var updateResult = productGroup.Update(brandId, brandName, categoryId, categoryName, command.Title, command.Description,
                                                command.IsSerialized, command.Specifications
                                               , Money.Create(command.OriginalPrice), command.width, command.Height,
                                                command.Length, command.Weight, command.Sku, command.Slug, command.BarCode);

        if (updateResult.Failed)
            return updateResult.Errors;

        await db.SaveAsync(ct);

        return Result.Updated;
    }

}
