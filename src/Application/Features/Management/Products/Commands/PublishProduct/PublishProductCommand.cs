
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.PublishProduct;

public sealed record PublishProductCommand( long ProductId) : IRequest<Result<Success>>
{

    internal ProductId ParsedProductId =>
    new(ProductId);
}
