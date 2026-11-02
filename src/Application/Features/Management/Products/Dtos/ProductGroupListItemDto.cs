
using Domain.ProductGroups.ValueObjects;

namespace Application.Features.Management.Products.Dtos;

public sealed record ProductGroupListItemDto
{
    public string Id { get; init; }
    public string Title { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Category { get; init; } = null!;
    public string Status { get; init; }
    public float AverageRating { get; init; }
    public int ProductsCount { get; set; }

    public ProductGroupListItemDto(ProductGroupId id, string title, string brand, string category,
        ProductAverageRating averageRating, ProductGroupState productsGroupStatus, int productsCount)
    {
        Id = id.ToString();
        Title = title;
        Brand = brand;
        Category = category;
        ProductsCount = productsCount;
        Status = productsGroupStatus.ToString();
        AverageRating = averageRating.GetRoundedValue();
    }
}
