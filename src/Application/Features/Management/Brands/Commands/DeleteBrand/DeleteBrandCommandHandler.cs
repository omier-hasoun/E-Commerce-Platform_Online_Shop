
namespace Application.Features.Management.Brands.Commands.DeleteBrand;

internal sealed class DeleteBrandCommandHandler(IAppDbContext db) : IRequestHandler<DeleteBrandCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteBrandCommand request, CancellationToken ct)
    {
        var brandId = request.ParsedBrandId;
        try
        {
            var result = await db.Brands.Where(x => x.Id == brandId)
                                    .ExecuteDeleteAsync(ct);
            if (result == 0)
                return ApplicationErrors.DeleteOperationFailed;
        }
        catch
        {
            return ApplicationErrors.DeleteOperationFailed;
        }

        return Result.Deleted;

    }
}
