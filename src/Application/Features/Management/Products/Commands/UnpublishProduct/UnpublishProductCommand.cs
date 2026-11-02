

using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.UnpublishProduct;

public sealed record UnpublishProductCommand(long ProductId) : IRequest<Result<Success>>
{
    internal ProductId ParsedProductId =>
        new(ProductId);
}
