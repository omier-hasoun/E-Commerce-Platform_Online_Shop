
using Domain.Categories;

namespace Application.Features.Management.Categories.Commands.DeleteCategory;

public sealed record DeleteCategoryCommand(long CategoryId) : IRequest<Result<Deleted>>
{
    public CategoryId ParsedCategoryId = new CategoryId(CategoryId);
}
