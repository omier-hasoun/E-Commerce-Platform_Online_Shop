using Application.Features.Management.Categories.Commands.CreateCategory;
using Application.Features.Management.Categories.Commands.DeleteCategory;
using Application.Features.Public.Categories.Queries.ListCategories;

namespace Api.Controllers.Categories;

[Route("api/categories")]
public sealed class CategoriesController(IMediator mediator) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> ListCategories(CancellationToken ct)
    {

        var result = await mediator.Send(new ListCategoriesQuery(), ct);

        return result.Match((response) => Ok(response), Problem);
    }
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand request, CancellationToken ct)
    {

        var result = await mediator.Send(request, ct);

        return result.Match((response) => Ok(response), Problem);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(long id, CancellationToken ct)
    {
        var result = await mediator.Send(new DeleteCategoryCommand(id), ct);

        return result.Match((response) => Ok(response), Problem);
    }
}
