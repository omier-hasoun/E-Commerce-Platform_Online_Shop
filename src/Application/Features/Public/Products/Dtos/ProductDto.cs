using Application.Common.Dtos;
using Domain.Common.ValueObjects;
using Domain.ProductGroups.Products;
using Domain.ProductGroups.ValueObjects;

namespace Application.Features.Public.Products.Dtos;

public sealed record ProductDto
{
    public long Id { get; }
    public double OriginalPrice { get; }
    public bool HasDiscount { get; }
    public double? DiscountPrice { get; }
    public byte? DiscountPercentage { get; }

    public bool InStock { get; }
    public string Slug { get; } = null!;
    public IReadOnlyCollection<ProductImageDto> Images { get; } = [];
    public IReadOnlyDictionary<string, string> VariantSpecifications { get; } = new Dictionary<string, string>();

    public ProductDto(ProductId id, Money originalPrice, bool hasDiscount, byte? discountPercentage, Money? discountPrice,
        IReadOnlyCollection<ProductImageDto> images, string slug, IReadOnlyDictionary<string, string> variantSpecifications, bool inStock)
    {
        Id = id.Value;
        OriginalPrice = (double)originalPrice.Value;
        Slug = slug;
        HasDiscount = hasDiscount;
        DiscountPrice = hasDiscount is false ? null : (double)discountPrice!.Value;
        DiscountPercentage = hasDiscount is false ? null : discountPercentage;
        Images = images;
        VariantSpecifications = variantSpecifications;
        InStock = inStock;
    }

}
