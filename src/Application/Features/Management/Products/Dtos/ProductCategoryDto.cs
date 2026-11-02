using Domain.Categories;

namespace Application.Features.Management.Products.Dtos;

public sealed record class ProductCategoryDto
{
    public long Id { get; init; }
    public string Name { get; init; }

    public ProductCategoryDto(CategoryId id, string name)
    {
        Name = name;
        Id = id.Value;
    }

}
