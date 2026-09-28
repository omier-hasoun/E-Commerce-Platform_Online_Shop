using Application.Features.Public.Categories.Dtos;
using Domain.Categories;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Application.Features.Management.Categories.Commands.CreateCategory;

internal sealed class CreateCategoryCommandHandler(IAppDbContext db, IIdGenerator<CategoryId> idGen) : IRequestHandler<CreateCategoryCommand, Result<List<CategoryListItem>>>
{
    public async Task<Result<List<CategoryListItem>>> Handle(CreateCategoryCommand request, CancellationToken ct)
    {
        var id = idGen.NewId();
        var result = Category.Create(id, request.Name, null);

        if (result.Failed)
            return result.Errors;

        db.Categories.Add(result.Value);

        await db.SaveAsync(ct);

        return await db.Categories.ListCategoriesAsync(db.Categories, ct);
    }
}
