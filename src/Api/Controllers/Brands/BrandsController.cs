
using Application.Features.Management.Brands.Commands.CreateBrand;
using Application.Features.Management.Brands.Commands.DeleteBrand;
using Application.Features.Public.Brands.Queries.ListBrands;

namespace Api.Controllers.Brands;

[Route("api/brands")]
public sealed class BrandsController(IMediator mediator) : ApiController
{


    [HttpGet]
    public async Task<IActionResult> ListBrands(CancellationToken ct)
    {

        var result = await mediator.Send(new ListBrandsQuery(), ct);

        return result.Match((response) => Ok(response), Problem);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBrand(CreateBrandCommand request , CancellationToken ct)
    {

        var result = await mediator.Send(request, ct);

        return result.Match((response) => Ok(response), Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBrand(Guid id, CancellationToken ct)
    {

        var result = await mediator.Send(new DeleteBrandCommand(id), ct);

        return result.Match((response) => Ok(response), Problem);
    }
}
