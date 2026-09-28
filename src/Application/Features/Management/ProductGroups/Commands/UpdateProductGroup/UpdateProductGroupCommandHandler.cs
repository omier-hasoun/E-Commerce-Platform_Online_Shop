using Domain.Brands;
using Domain.Categories;

namespace Application.Features.Management.ProductGroups.Commands.UpdateProductGroup;

internal sealed class UpdateProductGroupCommandHandler(IAppDbContext context) : IRequestHandler<UpdateProductGroupCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateProductGroupCommand command, CancellationToken ct)
    {

        ProductGroupId productGroupId = new(command.ProductGroupId);

        if (!command.HasChanges())
        {
            return Result.Updated;
        }

        var productGroup = await context.ProductGroups.FindAsync(productGroupId);

        if (productGroup is null)
        {
            return ApplicationErrors.NotFound.Product;
        }


        BrandId? brandId = command.BrandId is null ? null : new(command.BrandId.Value);

        CategoryId? categoryId = command.CategoryId is null ? null : new(command.CategoryId.Value);

        var brandName = await context.Brands.AsNoTracking()
                                .Where(x => x.Id == brandId)
                                .Select(x => x.Name)
                                .FirstOrDefaultAsync(ct);


        var categoryName = await context.Categories.AsNoTracking()
                                        .Where(x => x.Id == categoryId)
                                        .Select(x => x.Name)
                                        .FirstOrDefaultAsync(ct);

        if (brandName is null || categoryName is null)
            return brandName is null ? ApplicationErrors.NotFound.Brand : ApplicationErrors.NotFound.Category;

        var updateResult = productGroup.Update(brandId, brandName, categoryId, categoryName, command.Title, command.Description, command.IsSerialized, command.Specifications);

        if (updateResult.Failed)
            return updateResult.Errors;

        await context.SaveAsync(ct);

        return Result.Updated;
    }

}
