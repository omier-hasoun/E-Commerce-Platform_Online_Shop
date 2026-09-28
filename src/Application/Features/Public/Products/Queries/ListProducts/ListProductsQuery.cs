using Application.Common.Dtos;
using Application.Features.Public.Products.Dtos;
using Domain.Brands;
using Domain.Categories;

namespace Application.Features.Public.Products.Queries.ListProducts;

public sealed record ListProductsQuery : IRequest<Result<PaginatedList<ProductListItemDto>>>
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 30;

    public Guid? BrandId { get; init; } = null;
    public long? CategoryId { get; init; }= null;

    public int? MaxPrice { get; init; }= null;
    public string? SearchQuery { get; init; }= null;


    public bool OnlyGoodRatedProducts { get; init; } = false;
    public bool DiscountedProductsOnly { get; init; } = false;

    internal BrandId? ParsedBrandId =>
        BrandId.HasValue ? new BrandId(BrandId.Value) : null;

    internal CategoryId? ParsedCategoryId =>
        CategoryId.HasValue ? new CategoryId(CategoryId.Value) : null;
}
