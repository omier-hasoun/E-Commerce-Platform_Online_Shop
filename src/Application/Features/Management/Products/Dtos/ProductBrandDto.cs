using Domain.Brands;

namespace Application.Features.Management.Products.Dtos;

public sealed record ProductBrandDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;

    public ProductBrandDto(BrandId id, string name)
    {
        Name = name;
        Id = id.Value;
    }

}
