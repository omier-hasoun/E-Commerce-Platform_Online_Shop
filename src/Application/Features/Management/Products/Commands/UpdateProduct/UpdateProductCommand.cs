
using Application.Features.Management.Products.Dtos;
using Domain.Common.ValueObjects;

namespace Application.Features.Management.Products.Commands.UpdateProduct;

public sealed record UpdateProductCommand(
    long ProductId,
    Guid BrandId,
    long CategoryId,
    string Title,
    string Description,
    bool IsSerialized,
    decimal OriginalPrice,
    int width, int Height, int Length,
    int Weight, string Sku, string Slug, string BarCode,
    Dictionary<string, string>? Specifications = null)
: IRequest<Result<Updated>>
{

}
