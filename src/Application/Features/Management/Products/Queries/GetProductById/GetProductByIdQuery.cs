
using Application.Features.Management.Products.Dtos;
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(long ProductId) : IRequest<Result<ProductDto>>
{
    public ProductId ParsedProductId => new (ProductId);
}
