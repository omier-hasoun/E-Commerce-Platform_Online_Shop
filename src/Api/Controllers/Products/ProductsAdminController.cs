using Application.Features.Management.Products.Commands.UpdateProduct;
using Application.Features.Management.Products.Commands.PublishProduct;
using Application.Features.Management.Products.Commands.UnpublishProduct;
using Application.Features.Management.Products.Commands.ArchiveProduct;
using Api.Extensions;
using Application.Features.Management.Products.Commands.ApplyDiscount;
using Application.Features.Management.Products.Commands.RestockProduct;
using Application.Features.Management.Products.Commands.CreateProduct;
using Application.Features.Management.Products.Dtos;
using Application.Features.Management.Products.Commands.UpdateImages;

using Application.Features.Management.Products.Queries.GetProductById;
using Application.Features.Management.Products.Queries.ListProducts;

using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Api.Controllers.Products;

[Route("api/management/products")]
public sealed class ProductsAdminController(IMediator mediator, IUniqueImageNameProvider fileNameGen, IOptions<JsonOptions> mvcJsonOptions) : ApiController
{
    #region commands

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProductGroup([FromForm] CreateProductRequest request, CancellationToken ct)
    {

        // Deserialize images details
        List<ImageDetails>? imagesDetails = null;
        if (!string.IsNullOrEmpty(request.ImagesDetailsJson))
        {
            imagesDetails = JsonSerializer.Deserialize<List<ImageDetails>>(request.ImagesDetailsJson, mvcJsonOptions.Value.JsonSerializerOptions);
        }

        // Deserialize specifications
        Dictionary<string, string>? specifications = null;
        if (!string.IsNullOrEmpty(request.Specifications))
        {
            specifications = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Specifications);
        }

        var images = request.ImagesFiles?.Select(x => x.ToDto(fileNameGen.GenerateWithExtension(".webp")))
                                .ToList();

        var command = new CreateProductCommand
        (
            request.BrandId,
            request.CategoryId,
            request.Title,
            request.Description,
            request.IsSerialized,
            request.Price,
            request.Width,
            request.Height,
            request.Length,
            request.Weight,
            request.Sku,
            request.BarCode,
            request.Slug,
            request.Inventory,
            imagesDetails,
            images,
            specifications
        );



        var result = await mediator.Send(command, ct);

        return result.Match(response => Created(
                Url.Action(
                    nameof(GetProductById),
                    "ProductsManagement",
                    new { Id = response }),

                    new { Id = response }),

                    Problem
        );
    }

    [HttpPut("{productId}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct(
      long productId,
      [FromForm] UpdateProductRequest request,
      CancellationToken ct)
    {

        // Deserialize images details
        List<ImageDetails>? imagesDetails = null;
        if (!string.IsNullOrEmpty(request.ImagesDetailsJson))
        {
            imagesDetails = JsonSerializer.Deserialize<List<ImageDetails>>(request.ImagesDetailsJson, mvcJsonOptions.Value.JsonSerializerOptions);
        }

        // Deserialize specifications
        Dictionary<string, string>? specifications = null;
        if (!string.IsNullOrEmpty(request.Specifications))
        {
            specifications = JsonSerializer.Deserialize<Dictionary<string, string>>(request.Specifications);
        }

        var images = request.ImagesFiles?.Select(x => x.ToDto(fileNameGen.GenerateWithExtension(".webp")))
                                .ToList();

        {
            var result = await mediator.Send(new UpdateImagesCommand
            {
                ProductId = productId,
                NewFiles = images!,
                Images = imagesDetails!
            }, ct);

            if(result.Failed)
                return result.Match((response) => NoContent(), Problem);
        }

        {
            var result = await mediator.Send(new UpdateProductCommand(
                productId,
                request.BrandId,
                request.CategoryId,
                request.Title,
                request.Description,
                request.IsSerialized,
                request.Price,
                request.Width,
                request.Height,
                request.Length,
                request.Weight,
                request.Sku,
                request.Slug,
                request.BarCode,
                specifications));

            return result.Match((response) => NoContent(), Problem);
        }
    }


    [HttpPost("{productId}/archive")]
    public async Task<IActionResult> ArchiveProductGroup(long productId, CancellationToken ct)
    {

        var result = await mediator.Send(new ArchiveProductCommand(productId), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("{productId}/apply-discount")]
    public async Task<IActionResult> ApplyDiscount(long productId, [FromBody] ApplyDiscountRequest request, CancellationToken ct)
    {

        var result = await mediator.Send(new ApplyDiscountCommand(productId, request.DiscountExpiresOn, request.DiscountPercentage), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("{productId}/publish")]
    public async Task<IActionResult> PublishProduct(long productId, CancellationToken ct)
    {
        var result = await mediator.Send(new PublishProductCommand(productId), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("{productId}/unpublish")]
    public async Task<IActionResult> UnpublishProduct(long productId, CancellationToken ct)
    {
        var result = await mediator.Send(new UnpublishProductCommand(productId), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    #endregion


    #region queries

    [HttpGet]
    public async Task<IActionResult> ListProducts([FromQuery] ListProductsQuery request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);

        return result.Match((response) => Ok(response), Problem);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductById(long productId, CancellationToken ct)
    {

        var result = await mediator.Send(new GetProductByIdQuery(productId), ct);

        return result.Match((response) => Ok(response), Problem);
    }

    #endregion
}


//[HttpPost("{productGroupId}/products/{productId}/archive")]
//public async Task<IActionResult> ArchiveVariant(long productId, CancellationToken ct)
//{
//    var result = await mediator.Send(new , ct);

//    return result.Match((response) => NoContent(), Problem);
//}
