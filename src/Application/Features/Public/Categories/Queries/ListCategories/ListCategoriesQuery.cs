using Application.Features.Public.Categories.Dtos;

namespace Application.Features.Public.Categories.Queries.ListCategories;

public sealed record ListCategoriesQuery : IRequest<Result<List<CategoryListItem>>>
{

}
