using Application.Common.Dtos;
using Domain.Common.ValueObjects;
using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;

namespace Application.Features.Public.Products.Dtos;

public sealed record ProductDto
{
    public long Id { get; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string Brand { get; init; } = null!;
    public string Category { get; init; } = null!;
    public float AverageRating { get; init; }

    public IReadOnlyDictionary<string, string> Specifications { get; init; } = null!;
    public double OriginalPrice { get; }
    public bool HasDiscount { get; }
    public double? DiscountPrice { get; }
    public byte? DiscountPercentage { get; }
    public bool IsSerialized { get; }

    public bool InStock { get; }
    public string Slug { get; } = null!;
    public IReadOnlyCollection<ProductImageDto>? Images { get; } = [];

    public int StockQuantity { get; }

    public DateOnly? DiscountExpiresOn { get; }
    public ProductDto(ProductId id, string title, string description, IReadOnlyDictionary<string, string> specifications,
        string brand, string category, ProductAverageRating averageRating, Money originalPrice, bool hasDiscount,  DateOnly? discountExpiresOn, byte? discountPercentage, Money? discountPrice,
        IReadOnlyCollection<ProductImageDto>? images, string slug, bool inStock, int stockQuantity, bool isSerialized)
    {
        DiscountExpiresOn = discountExpiresOn;
        Id = id.Value;
        Title = title;
        Description = description;
        Specifications = specifications;
        Brand = brand;
        Category = category;
        AverageRating = averageRating.GetRoundedValue();
        OriginalPrice = (double)originalPrice.Value;
        Slug = slug;
        HasDiscount = hasDiscount;
        DiscountPrice = hasDiscount is false ? null : (double)discountPrice!.Value;
        DiscountPercentage = hasDiscount is false ? null : discountPercentage;
        Images = images;
        Specifications = specifications;
        InStock = inStock;
        StockQuantity = stockQuantity;
        IsSerialized = isSerialized;
    }

}
