
using Application.Features.Public.Brands.Queries.Dtos;

namespace Application.Features.Public.Brands.Queries.ListBrands;

public sealed class ListBrandsQueryHandler(IAppDbContext context) : IRequestHandler<ListBrandsQuery, Result<List<BrandListItem>>>
{
    public async Task<Result<List<BrandListItem>>> Handle(ListBrandsQuery request, CancellationToken ct)
    {

        var brands = await context.Brands.AsNoTracking()
                                         .Where(x => x.IsActive)
                                         .Select(x => new BrandListItem(x.Id, x.Name))
                                         .ToListAsync(ct);

        return brands;
    }
}
