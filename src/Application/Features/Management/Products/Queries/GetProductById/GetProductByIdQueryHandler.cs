
using Application.Features.Management.Products.Dtos;
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler(IAppDbContext context, IProductThumbnailUrlProvider productThumbnailUrlProvider) : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var productId = request.ParsedProductId;

        var group = await context.ProductGroups.AsNoTracking()
            .Include(pg => pg.MainProduct)
                .ThenInclude(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.MainProductId == productId, ct);

        if (group is null)
        {
            return ApplicationErrors.NotFound.Product;
        }

        return new ProductDto(
            group.MainProduct!.Id,
            group.Title,
            group.Description,
            group.Specifications,
            group.CategoryId,
            group.BrandId,
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
            group.IsSerialized,
            group.MainProduct.BarCode,
            group.MainProduct.Sku,
            group.MainProduct.Status.ToString(),
            group.MainProduct.Status == ProductState.Published,
            group.MainProduct.Width,
            group.MainProduct.Height,
            group.MainProduct.Length,
            group.MainProduct.Weight);

    }
}
