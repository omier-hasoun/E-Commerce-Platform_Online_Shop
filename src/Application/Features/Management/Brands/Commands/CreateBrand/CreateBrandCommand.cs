using Application.Features.Public.Brands.Queries.Dtos;

namespace Application.Features.Management.Brands.Commands.CreateBrand;

public sealed record CreateBrandCommand
(
    string Name,
    string Description,
    string Slug,
    string CompanyName,
    string? LogoUrl
) : IRequest<Result<List<BrandListItem>>>;
