
using Application.Features.Public.Categories.Dtos;

namespace Application.Features.Public.Categories.Queries.ListCategories;

internal sealed class ListCategoriesQueryHandler(IAppDbContext db) : IRequestHandler<ListCategoriesQuery, Result<List<CategoryListItem>>>
{
    public async Task<Result<List<CategoryListItem>>> Handle(ListCategoriesQuery request, CancellationToken ct)
    {
        return await db.Categories.ListCategoriesAsync(db.Categories, ct);
    }
}
