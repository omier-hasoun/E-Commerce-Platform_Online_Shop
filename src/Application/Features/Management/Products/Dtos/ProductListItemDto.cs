
using Domain.Common.ValueObjects;
using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;

namespace Application.Features.Management.Products.Dtos;

public sealed record ProductListItemDto
{
    public long Id { get; }
    public string Title { get; init; } = null!;
    public double OriginalPrice { get; init; }
    public string Brand { get; init; } = null!;
    public float AverageRating { get; init; }
    public string? ThumbnailUrl { get;  }
    public string Status {get;}
    public byte? DiscountPercentage { get; init; }
    public double? DiscountPrice { get; init; }
    public bool HasDiscount { get; init; }
    public bool InStock { get; }

    public ProductListItemDto(ProductId id, string title, Money originalPrice, string brand, ProductAverageRating averageRating,
        string? thumbnailUrl, bool hasDiscount, Money? discountPrice, byte? discountPercentage, bool inStock, string status)
    {
        Id = id.Value;
        Title = title;
        Brand = brand;
        HasDiscount = hasDiscount;
        InStock = inStock;
        OriginalPrice = (double)originalPrice.Value;
        DiscountPrice = hasDiscount ? (double)discountPrice!.Value : null;
        ThumbnailUrl = thumbnailUrl;
        AverageRating = averageRating.GetRoundedValue();
        DiscountPercentage = hasDiscount ? discountPercentage : null;
        Status = status;
    }
}
