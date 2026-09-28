namespace Application.Features.Public.Categories.Dtos;

public sealed record ParentCategoryListItem(string Id, string Name, List<CategoryListItem> Children)
{

}
