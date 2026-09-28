using Application.Features.Management.Categories.Commands.DeleteCategory;
using Domain.Categories;

namespace Application.Features.Management.Categories.Commands.CreateCategory;

internal sealed class DeleteCategoryQueryHandler(IAppDbContext db) : IRequestHandler<DeleteCategoryCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteCategoryCommand request, CancellationToken ct)
    {
        try
        {
            var result = await db.Categories.Where(x => x.Id == request.ParsedCategoryId)
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
