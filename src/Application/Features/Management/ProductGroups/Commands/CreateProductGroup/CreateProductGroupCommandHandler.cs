using Domain.Brands;
using Domain.Categories;

namespace Application.Features.Management.ProductGroups.Commands.CreateProductGroup;

internal sealed class CreateProductGroupCommandHandler(IAppDbContext context, IIdGenerator<ProductGroupId> idGen) : IRequestHandler<CreateProductGroupCommand, Result<long>>
{
    public async Task<Result<long>> Handle(CreateProductGroupCommand command, CancellationToken ct)
    {


        BrandId brandId = new(command.BrandId);

        CategoryId categoryId = new(command.CategoryId);

        ProductGroupId productId = idGen.NewId();

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


        var createProductResult = ProductGroup.Create(
            productId,
            brandId,
            brandName,
            categoryId,
            categoryName,
            command.Title,
            command.Description,
            command.IsSerialized,
            command.Specifications
        );

        if (createProductResult.Failed)
        {
            return createProductResult.Errors;
        }

        context.ProductGroups.Add(createProductResult.Value);

        await context.SaveAsync(ct);

        return productId.Value;
    }

}
