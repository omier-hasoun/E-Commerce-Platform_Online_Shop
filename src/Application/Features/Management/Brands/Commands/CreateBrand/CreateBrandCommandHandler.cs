using Application.Features.Public.Brands.Queries.Dtos;
using Domain.Brands;

namespace Application.Features.Management.Brands.Commands.CreateBrand;

internal sealed class CreateBrandCommandHandler(IAppDbContext db, IIdGenerator<BrandId> idGen) : IRequestHandler<CreateBrandCommand, Result<List<BrandListItem>>>
{
    public async Task<Result<List<BrandListItem>>> Handle(CreateBrandCommand request, CancellationToken ct)
    {
        var id = idGen.NewId();
        var result = Brand.Create(
            id,
            request.Name,
            request.Description,
            request.CompanyName,
            request.Slug,
            request.LogoUrl);

        if (result.Failed)
        {
            return result.Errors;
        }

        db.Brands.Add(result.Value);

        await db.SaveAsync(ct);
        return await db.Brands.AsNoTracking()
                            .Where(x => x.IsActive)
                            .Select(x => new BrandListItem(x.Id, x.Name))
                            .ToListAsync(ct);
    }

}
