using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;

namespace Application.Features.Public.Products.Dtos;

public sealed record ProductGroupDto
{
    public long RequestedProductId {get;}
    public long MainProductId { get; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Category { get; init; } = null!;
    public float AverageRating { get; init; }

    public IReadOnlyDictionary<string, string> Specifications { get; init; } = null!;
    public IEnumerable<ProductDto> Products { get; init; } = null!;

    public ProductGroupDto(ProductId requestedProductId, ProductId? mainProductId, string title, string description, IReadOnlyDictionary<string, string> specifications,
        string brand, string category, ProductAverageRating averageRating, IEnumerable<ProductDto> products)
    {
        RequestedProductId = requestedProductId.Value;
        MainProductId = mainProductId!.Value.Value;
        Title = title;
        Description = description;
        Specifications = specifications;
        Brand = brand;
        Category = category;
        AverageRating = averageRating.GetRoundedValue();
        Products = products;

    }

}
