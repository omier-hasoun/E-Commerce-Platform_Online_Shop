
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
            .GetPubishedProductGroups()
            .ApplyBrandFilter(request.BrandId)
            .ApplyCategoryFilter(request.CategoryId)
            .ApplySearchTextFilter(request.SearchQuery)
            .ApplyDiscountedProductsFilter(request.DiscountedProductsOnly)
            .ApplyRatingFilter(request.OnlyGoodRatedProducts)
            .ApplyMaxPriceFilter(request.MaxPrice)
            .OrderByDescending(x => x.Products.Any(p => p.Inventory.StockQuantity> 0))
            .ThenByDescending(x => x.Products.Any(p => p.HasDiscount))
            .ThenBy(x=> x.Id);


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

    /*

    info: Microsoft.EntityFrameworkCore.Database.Command[20101]
          Executed DbCommand (31ms) [Parameters=[@searchText_contains='?' (Size = 60), @p='?' (DbType = Int32), @p3='?' (DbType = Int32)], CommandType='Text', CommandTimeout='30']
          SELECT [p0].[Id], [p1].[Id], [p1].[Title], [p0].[OriginalPrice], [p1].[BrandName], [p1].[AverageRating], (
              SELECT TOP(1) JSON_VALUE([i0].[value], '$.FileName')
              FROM OPENJSON([p0].[Images], '$') AS [i0]
              ORDER BY CAST([i0].[key] AS int)), [p0].[HasDiscount], [p0].[DiscountPrice], [p0].[DiscountPercentage], CASE
              WHEN [i].[StockQuantity] > 0 THEN CAST(1 AS bit)
              ELSE CAST(0 AS bit)
          END
          FROM (
              SELECT [p].[Id], [p].[BrandName], [p].[FeaturedProductId], [p].[Title], [p].[AverageRating]
              FROM [ProductGroups] AS [p]
              WHERE [p].[Status] = 2 AND [p].[NormalizedTitle] LIKE @searchText_contains ESCAPE N'\'
              ORDER BY (SELECT 1)
              OFFSET @p ROWS FETCH NEXT @p3 ROWS ONLY
          ) AS [p1]
          LEFT JOIN [Products] AS [p0] ON [p1].[FeaturedProductId] = [p0].[Id]
          LEFT JOIN [Inventories] AS [i] ON [p0].[Id] = [i].[ProductId]
    */

}
