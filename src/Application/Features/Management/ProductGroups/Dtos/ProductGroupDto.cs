using Application.Common.Dtos;
using Domain.Brands;
using Domain.Categories;
using Domain.ProductGroups.ValueObjects;

namespace Application.Features.Management.ProductGroups.Dtos;

public sealed record ProductGroupDto
{
    public string Id { get; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;

    public float AverageRating { get; init; }
    public ProductCategoryDto Category { get; init; } = null!;
    public ProductBrandDto Brand { get; init; }
    public IReadOnlyDictionary<string, string> Specifications { get; init; } = null!;
    public IReadOnlyCollection<ProductListItemDto> Products { get; init; } = null!;
    public AuditedUserDto LastModifiedInfo { get; init; } = null!;


    public ProductGroupDto(ProductGroupId id, string title, string description, IReadOnlyDictionary<string, string> Specifications,
        BrandId brandId, string brandName, CategoryId categoryId, string categoryName, ProductAverageRating averageRating,
        DateTime lastModifiedAt, Guid lastModifiedBy, string lastModifiedByUserName,
        IReadOnlyCollection<ProductListItemDto> products)
    {
        Id = id.ToString();
        Title = title;
        Description = description;
        Specifications = Specifications;
        Brand = new(brandId, brandName);
        Category = new ProductCategoryDto(categoryId, categoryName);

        Products = products;
        AverageRating = averageRating.GetRoundedValue();
        LastModifiedInfo = new(lastModifiedBy, lastModifiedByUserName, lastModifiedAt);


    }
}
