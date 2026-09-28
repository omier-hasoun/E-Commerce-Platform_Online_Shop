using Application.Features.Public.Products.Dtos;
using Domain.ProductGroups.Products;

namespace Application.Features.Public.Products.Queries.GetProductById;

internal sealed class GetProductByIdQueryHandler(IAppDbContext context, IProductThumbnailUrlProvider productThumbnailUrlProvider) : IRequestHandler<GetProductByIdQuery, Result<ProductGroupDto>>
{
    public async Task<Result<ProductGroupDto>> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var productId = request.ParsedProductId;

        var group = await context.ProductGroups
            .AsNoTracking()
            .Include(pg => pg.Products)
                .ThenInclude(p => p.Inventory)
            .Where(pg => pg.MainProductId != null &&
                        pg.Products.Any(p => p.Id == productId && p.Status == ProductState.Published))
            .FirstOrDefaultAsync(ct);

            if (group is null)
            {
                return ApplicationErrors.NotFound.Product;
            }

        var productGroupDto = new ProductGroupDto(
            productId,
            group.MainProductId!,
            group.Title,
            group.Description,
            group.Specifications,
            group.BrandName,
            group.CategoryName,
            group.AverageRating,
            group.Products.Select(p => {

                List<ProductImageDto> images;
                if(p.Images == null || p.Images.Count == 0)
                {
                    images = new (1) {  new ProductImageDto(productThumbnailUrlProvider.GetDefaultThumbnailUrl(ProductThumbnailSize.Medium), 1)};
                }
                else
                {
                    images = p.Images.Select(img => new ProductImageDto(
                                        productThumbnailUrlProvider.GetRelativeUrl(img.FileName,
                                        ProductThumbnailSize.Large), img.SortOrder)).ToList();
                }

                return new ProductDto(
                p.Id,
                p.OriginalPrice,
                p.HasDiscount,
                p.DiscountPercentage,
                p.DiscountPrice,
                images,
                p.Slug,
                p.VariantSpecifications,
                p.Inventory != null && p.Inventory.StockQuantity > 0);
            })
        );

        return productGroupDto;
    }
}
