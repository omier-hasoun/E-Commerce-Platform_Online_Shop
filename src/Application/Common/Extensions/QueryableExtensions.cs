
using Application.Features.Public.Categories.Dtos;
using Domain.Brands;
using Domain.Carts;
using Domain.Carts.CartItems;
using Domain.Categories;
using Domain.Common.ValueObjects;
using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;
using Shared.Helpers;

namespace Application.Common.Extensions;

internal static class QueryableExtensions
{
    #region products
    public static IQueryable<Product> ApplyDiscountedProductsFilter(
        this IQueryable<Product> query, bool apply)
    {
        if (apply is false)
            return query;

        return query.Where(p => p.HasDiscount);
    }

    public static IQueryable<Product> ApplyProductStatusesFilter(
    this IQueryable<Product> query, List<ProductState> statuses)
    {
        if (statuses is null || statuses.Count == 0)
            return query;

        return query.Where(p => statuses.Contains(p.Status));

    }

    public static IQueryable<Product> GetPubishedProducts(this IQueryable<Product> query)
    {
        return query.Where(x => x.Status == ProductState.Published);
    }

    public static IQueryable<Product> ApplyMaxPriceFilter(this IQueryable<Product> query, int? maxPrice)
    {
        if (!maxPrice.HasValue || maxPrice.Value < 0)
            return query;

        var moneyMaxPrice = Money.Create(maxPrice.Value).Value;

        //return query.Where(p => p.OriginalPrice <= moneyMaxPrice || (p.HasActiveDiscount && p.PriceAfterDiscount! <= moneyMaxPrice));
        return null;
    }
    #endregion

    #region productGroups
    public static IQueryable<ProductGroup> ApplySearchTextFilter(
        this IQueryable<ProductGroup> query,
        string? searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return query.OrderBy(x => x.Id);

        return query.Where(
            x => x.NormalizedTitle.Contains(searchText));
    }
    public static IQueryable<ProductGroup> ApplyBrandFilter(this IQueryable<ProductGroup> query, Guid? brandId)
    {
        if (!brandId.HasValue)
            return query;

        var parsedId = new BrandId(brandId.Value);

        return query.Where(group => group.BrandId == parsedId);
    }
    public static IQueryable<ProductGroup> ApplyCategoryFilter(this IQueryable<ProductGroup> query, long? categoryId)
    {
        if (!categoryId.HasValue)
            return query;

        var parsedId = new CategoryId(categoryId.Value);
        return query.Where(group => group.CategoryId == parsedId);
    }

    public static IQueryable<ProductGroup> ApplyStatusesFilter(this IQueryable<ProductGroup> query, List<ProductGroupState>? statuses)
    {
        if (statuses is null || statuses.Count == 0)
            return query;

        return query.Where(x => statuses.Contains(x.Status));
    }
    public static IQueryable<ProductGroup> ApplyDiscountedProductsFilter(this IQueryable<ProductGroup> query, bool discountedProductsOnly)
    {
        if(!discountedProductsOnly)
            return query;

        return query.Where(x => x.Products.Any(p => p.HasDiscount));
    }
    public static IQueryable<ProductGroup> ApplyMaxPriceFilter(this IQueryable<ProductGroup> query, int? maxPrice)
    {
        if(maxPrice.HasValue && maxPrice > 0)
        {
            var parsedMaxPrice =  Money.Create(maxPrice.Value);
            return query.Where(g => g.MainProduct!.CurrentPrice <= parsedMaxPrice);
        }
        return query;
    }
    public static IQueryable<ProductGroup> ApplyRatingFilter(this IQueryable<ProductGroup> query, bool onlyGoodRated)
    {
        if(!onlyGoodRated)
            return query;

        var goodRating =  ProductAverageRating.From(3).Value;
        return query.Where(x => x.AverageRating > goodRating);
    }

    public static IQueryable<ProductGroup> GetPubishedProductGroups(this IQueryable<ProductGroup> query)
    {
        return query.Where(x => x.Status == ProductGroupState.Published && x.MainProductId != null);
    }

    #endregion

    #region carts

    public static IQueryable<Cart?> GetUserCartQuery(this IQueryable<Cart> query, CurrentUser identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        if (identity.IsUser)
        {
            query = query.Where(x => x.UserId == identity.UserId);
        }
        else
        {
            query = query.Where(x => x.GuestId == identity.GuestId);
        }

        return query;
    }

    #endregion

    public static IQueryable<Order?> UserAbandonedOrderQuery(this IQueryable<Order> query, CurrentUser identity)
    {
        ArgumentNullException.ThrowIfNull(identity);

        if (identity.IsUser)
        {
            query = query.Where(x => x.UserId == identity.UserId);
        }
        else
        {
            query = query.Where(x => x.GuestId == identity.GuestId);
        }

        return query.Where(x => x.Status == OrderState.Pending);
    }

    public static async Task<PaginatedList<TResult>> ToPaginatedListAsync<TResult>(
        this IQueryable<TResult> query,
        int page,
        int size,
        CancellationToken ct)
    {
        int skip = ((page - 1) * size);

        var list = await query.Skip(skip)
                              .Take(size + 1)
                              .ToListAsync(ct);
        if (list is null || list.Count == 0)
        {
            return PaginatedList<TResult>.Empty;
        }

        var hasMore = list.Count > size;
        if (hasMore)
            list.RemoveAt(list.Count - 1);

        return list.ToPaginatedList(page, hasMore);
    }

    public static async Task<Result<List<CategoryListItem>>> ListCategoriesAsync(this IQueryable<Category> query, DbSet<Category> db, CancellationToken ct)
    {
        var q = query.AsNoTracking()
                                .Where(x => x.ParentCategoryId == null)
                                .Select(x =>
                                new CategoryListItem(x.Id.ToString(), x.Name)
                                );

        var categories = await q.ToListAsync(ct);

        return categories;
    }

}
