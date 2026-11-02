
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.ApplyDiscount;

public sealed record ApplyDiscountCommand(long ProductId, DateOnly DiscountExpiresOn, byte DiscountPercentage)
: IRequest<Result<Success>>
{
    internal ProductId ParsedProductId =>
    new(ProductId);
}
