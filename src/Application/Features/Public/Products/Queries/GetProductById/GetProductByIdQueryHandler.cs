using Application.Features.Public.Products.Dtos;
using Domain.ProductGroups.Products;

namespace Application.Features.Public.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler(IAppDbContext context, IProductThumbnailUrlProvider productThumbnailUrlProvider) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var productId = request.ParsedProductId;

        var group = await context.ProductGroups
            .AsNoTracking()
            .Where(pg => pg.MainProductId == productId && pg.MainProduct!.Status == ProductState.Published)
            .Include(pg => pg.MainProduct)
                .ThenInclude(p => p.Inventory)
            .FirstOrDefaultAsync(ct);

        if (group is null)
        {
            return ApplicationErrors.NotFound.Product;
        }

        return new ProductDto(
            group.MainProduct!.Id,
            group.Title,
            group.Description,
            group.Specifications,
            group.BrandName,
            group.CategoryName,
            group.AverageRating,
            group.MainProduct!.OriginalPrice,
            group.MainProduct!.HasDiscount,
            group.MainProduct.HasDiscount ? group.MainProduct.DiscountExpiresOn : null,
            group.MainProduct!.DiscountPercentage,
            group.MainProduct!.DiscountPrice,
            group.MainProduct.Images?.Select(img =>
            {
                var image = productThumbnailUrlProvider.GetRelativeUrl(img.FileName, ProductThumbnailSize.Large);
                return new ProductImageDto(img.Id, image, img.SortOrder);
            }).ToList(),
            group.MainProduct.Slug,
            group.MainProduct.Inventory!.StockQuantity > 0,
            group.MainProduct.Inventory!.StockQuantity,
            group.IsSerialized);

    }
}
