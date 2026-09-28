using Application.Features.Public.Categories.Dtos;

namespace Application.Features.Management.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : IRequest<Result<List<CategoryListItem>>>
{

}
