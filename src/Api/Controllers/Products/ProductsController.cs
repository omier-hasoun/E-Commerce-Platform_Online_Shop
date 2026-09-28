
using Application.Features.Public.Products.Queries.GetProductById;
using Application.Features.Public.Products.Queries.ListProducts;
using MediatR;

namespace Api.Controllers.Products;

[Route("api/products")]
public sealed class ProductsController(IMediator mediator) : ApiController
{


    [HttpGet()]
    public async Task<IActionResult> ListProducts([FromQuery] ListProductsQuery request, CancellationToken ct)
    {

        var result = await mediator.Send(request, ct);

        return result.Match((response) => Ok(response), Problem);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] long id, CancellationToken ct)
    {

        var result = await mediator.Send(new GetProductByIdQuery(id), ct);

        return result.Match((response) => Ok(response), Problem);
    }


}
