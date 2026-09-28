using Application.Features.Public.Products.Dtos;
using Domain.ProductGroups.Products;

namespace Application.Features.Public.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(long ProductId) : IRequest<Result<ProductGroupDto>>
{
    public ProductId ParsedProductId { get; } = new(ProductId);

}
