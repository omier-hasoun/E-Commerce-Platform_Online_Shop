
using Application.Features.Public.Brands.Queries.Dtos;

namespace Application.Features.Public.Brands.Queries.ListBrands;

public sealed record ListBrandsQuery : IRequest<Result<List<BrandListItem>>>
{
}
