using Domain.Brands;

namespace Application.Features.Management.Brands.Commands.DeleteBrand;

public sealed record DeleteBrandCommand(Guid Id) : IRequest<Result<Deleted>>
{
    public BrandId ParsedBrandId => new BrandId(Id);
}
