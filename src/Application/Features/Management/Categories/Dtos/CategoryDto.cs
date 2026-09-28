namespace Application.Features.Management.Categories.Dtos;

public sealed record CategoryDto(long Id, string name, long ParentCategoryId)
{

}
