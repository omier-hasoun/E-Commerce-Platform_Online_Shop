
using System.Security.Cryptography.X509Certificates;
using Application.Features.Public.Products.Dtos;
using Domain.Common.ValueObjects;

namespace Application.Features.Public.Products.Queries.ListProducts;

internal sealed class ListProductsQueryHandler(IAppDbContext context, IProductThumbnailUrlProvider productThumbnailsUrlProvider) : IRequestHandler<ListProductsQuery, Result<PaginatedList<ProductListItemDto>>>
{

    public async Task<Result<PaginatedList<ProductListItemDto>>> Handle(ListProductsQuery request, CancellationToken ct)
    {
        if (request.Size > 50)
        {
            return ApplicationErrors.Validation.PageSizeTooBig;
        }


        var query = context.ProductGroups.AsNoTracking()
            .ApplyFilterByPublishedProducts()
            .ApplyBrandFilter(request.BrandId)
            .ApplyCategoryFilter(request.CategoryId)
            .ApplySearchTextFilter(request.SearchQuery)
            .ApplyDiscountedProductsFilter(request.DiscountedProductsOnly)
            .ApplyRatingFilter(request.OnlyGoodRatedProducts)
            .ApplyMaxPriceFilter(request.MaxPrice)
            .OrderByDescending(x => x.Products.Any(p => p.Inventory.StockQuantity > 0))
            .ThenBy(x => x.Id);


        var prjoctionQuery = query.Select(g => new ProductListItemDto(
            g.MainProduct!.Id,
            g.Title,
            g.MainProduct.OriginalPrice,
            g.BrandName,
            g.AverageRating,
            productThumbnailsUrlProvider.GetRelativeUrl(g.MainProduct.Images.FirstOrDefault().FileName, ProductThumbnailSize.Small),
            g.MainProduct.HasDiscount,
            g.MainProduct.DiscountPrice,
            g.MainProduct.DiscountPercentage,
            inStock: g.MainProduct.Inventory.StockQuantity > 0
        ));


        var result = await prjoctionQuery.ToPaginatedListAsync(
            request.Page,
            request.Size,
            ct);

        return result;
    }


}
