
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.ArchiveProduct;

public sealed record ArchiveProductCommand(long ProductId, bool ResetProductsStock = true) : IRequest<Result<Success>>
{

    internal ProductId ParsedProductId =>
        new(ProductId);
}
