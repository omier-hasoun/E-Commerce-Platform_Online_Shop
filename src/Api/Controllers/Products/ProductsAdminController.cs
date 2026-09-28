
using Application.Features.Management.ProductGroups.Commands.CreateProductGroup;
using Application.Features.Management.ProductGroups.Commands.AddProduct;
using Application.Features.Management.ProductGroups.Commands.AddImages;
using Application.Features.Management.ProductGroups.Commands.UpdateProductGroup;
using Application.Features.Management.ProductGroups.Queries.ListProductGroups;
using Application.Features.Management.ProductGroups.Queries.GetProductsGroupById;
using Application.Features.Management.ProductGroups.Commands.PublishProduct;
using Application.Features.Management.ProductGroups.Commands.UnpublishProduct;
using Application.Features.Management.ProductGroups.Commands.ArchiveProductGroup;
using Application.Features.Management.ProductGroups.Queries.GetProductById;
using Application.Features.Management.ProductGroups.Commands.UpdateImagesSortOrder;
using Api.Extensions;
using Application.Features.Management.ProductGroups.Commands.RemoveImages;
using Application.Features.Management.ProductGroups.Commands.ApplyDiscount;
using Application.Features.Management.ProductGroups.Commands.RestockProduct;

namespace Api.Controllers.Products;

[Route("api/management/")]
public sealed class ProductsAdminController(IMediator mediator, IUniqueImageNameProvider fileNameGen) : ApiController
{
    #region commands

    [HttpPost("product-group")]
    public async Task<IActionResult> CreateProductGroup([FromBody] CreateProductGroupCommand request, CancellationToken ct)
    {

        var result = await mediator.Send(request, ct);

        return result.Match(response => Created(
                Url.Action(
                    nameof(GetProductGroupById),
                    "ProductsManagement",
                    new { Id = response }),

                    new { Id = response }),

                    Problem
        );
    }

    [HttpPatch("product-group/{id}")]
    public async Task<IActionResult> UpdateProductGroup(long id, [FromBody] UpdateProductGroupRequest request, CancellationToken ct)
    {
        var command = new UpdateProductGroupCommand(
            id,
            request.BrandId,
            request.CategoryId,
            request.Title,
            request.Description,
            request.IsSerialized,
            request.Specifications);

        var result = await mediator.Send(command, ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("product-group/{id}/products")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddProduct(long id, [FromForm] AddProductRequest request, CancellationToken ct)
    {
        List<FileUploadDto>? imagesDto = null;
        if (request.Images != null && request.Images.Count > 0)
        {
            imagesDto = new(request.Images.Count);
            request.Images.ForEach(image => imagesDto.Add(

                new FileUploadDto
                {
                    InternalFileName = fileNameGen.Generate() + ".webp",
                    ContentStream = image.OpenReadStream(),
                    ContentLength = image.Length,
                    MediaType = image.ContentType,
                    OriginalFileName = image.FileName,
                }
            ));
        }



        var command = new AddProductCommand(
            id,
            request.Price,
            request.Width,
            request.Height,
            request.Length,
            request.Weight,
            request.Sku,
            request.Slug,
            request.BarCode,
            request.Specifications,
            imagesDto,
            request.StockPerWarehouse);

        var result = await mediator.Send(command, ct);

        return result.Match((response) =>

        Created(
            Url.Action(nameof(GetProductById), "ProductsManagement", new { Id = response }),
            new { Id = response }),

            Problem
        );
    }


    [HttpPost("product-group/{id}/publish")]
    public async Task<IActionResult> PublishProductGroup(long id, CancellationToken ct)
    {

        var result = await mediator.Send(new PublishProductGroupCommand(id), ct);

        return result.Match((response) => NoContent(), Problem);
    }
    [HttpPost("product-group/{id}/unpublish")]
    public async Task<IActionResult> UnpublishProductGroup(long id, CancellationToken ct)
    {

        var result = await mediator.Send(new UnpublishProductGroupCommand(id), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("product-group/{id}/archive")]
    public async Task<IActionResult> ArchiveProductGroup(long id, CancellationToken ct)
    {

        var result = await mediator.Send(new ArchiveProductGroupCommand(id), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("product-group/{productGroupId}/products/{productId}/apply-discount")]
    public async Task<IActionResult> ApplyDiscount(long productGroupId, long productId, [FromBody] ApplyDiscountRequest request, CancellationToken ct)
    {

        var result = await mediator.Send(new ApplyDiscountCommand(productGroupId, productId, request.DiscountExpiresOn, request.DiscountPercentage), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("product-group/{productGroupId}/products/{productId}/publish")]
    public async Task<IActionResult> PublishProduct(long productGroupId, long productId, CancellationToken ct)
    {
        var result = await mediator.Send(new PublishProductCommand(productGroupId, productId), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("product-group/{productGroupId}/products/{productId}/unpublish")]
    public async Task<IActionResult> UnpublishProduct(long productGroupId, long productId, CancellationToken ct)
    {
        var result = await mediator.Send(new UnpublishProductCommand(productGroupId, productId), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPost("products/{productId}/inventory/{warehouseId}")]
    public async Task<IActionResult> RestockProduct(long warehouseId, long productId, [FromBody] int stockQuantity, CancellationToken ct)
    {
        var result = await mediator.Send(new RestockProductCommand(warehouseId, productId, stockQuantity), ct);

        return result.Match((response) => NoContent(), Problem);
    }

    [HttpPut("product-group/{productGroupId}/products/{productId}/images")]
    public async Task<IActionResult> UpdateImagesSortOrder(long productGroupId, long productId, [FromBody] UpdateImagesSortOrderRequest request, CancellationToken ct)
    {

        var result = await mediator.Send(new UpdateImagesSortOrderCommand(productGroupId, productId, request.Images), ct);

        return result.Match((response) => NoContent(), Problem);

    }

    [HttpPost("product-group/{productGroupId}/products/{productId}/images")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddImages(long productGroupId, long productId, [FromForm] IReadOnlyCollection<IFormFile> files, CancellationToken ct)
    {
        List<FileUploadDto> images = new(files.Count);

        foreach (var file in files)
        {
            images.Add(file.ToDto(fileNameGen.GenerateWithExtension(".webp")));
        }

        var result = await mediator.Send(new AddImagesCommand(productGroupId, productId, images), ct);

        return result.Match((response) => NoContent(), Problem);

    }

    [HttpDelete("product-group/{productGroupId}/products/{productId}/images")]
    public async Task<IActionResult> RemoveImages(long productGroupId, long productId, [FromBody] List<string> fileNames, CancellationToken ct)
    {

        var result = await mediator.Send(new RemoveImagesCommand(productGroupId, productId, fileNames), ct);

        return result.Match((response) => NoContent(), Problem);

    }

    #endregion


    #region queries

    [HttpGet("product-group")]
    public async Task<IActionResult> ListProducts([FromQuery] ListProductGroupsQuery request, CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);

        return result.Match((response) => Ok(response), Problem);
    }

    [HttpGet("product-group/{productGroupId}")]
    public async Task<IActionResult> GetProductGroupById(long productGroupId, CancellationToken ct)
    {

        var result = await mediator.Send(new GetProductsGroupByIdQuery(productGroupId), ct);

        return result.Match((response) => Ok(response), Problem);
    }

    [HttpGet("products/{productId}")]
    public async Task<IActionResult> GetProductById(long productId, CancellationToken ct)
    {

        var result = await mediator.Send(new GetProductByIdQuery(productId), ct);

        return result.Match((response) => Ok(response), Problem);
    }

    #endregion
}


//[HttpPost("{productGroupId}/products/{productId}/archive")]
//public async Task<IActionResult> ArchiveVariant(long productGroupId, long productId, CancellationToken ct)
//{
//    var result = await mediator.Send(new , ct);

//    return result.Match((response) => NoContent(), Problem);
//}
