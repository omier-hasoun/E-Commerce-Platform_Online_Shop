
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.ApplyDiscount;

internal sealed class ApplyDiscountCommandHandler(IAppDbContext context) : IRequestHandler<ApplyDiscountCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(ApplyDiscountCommand request, CancellationToken ct)
    {
        ProductId productId = request.ParsedProductId;

        var productGroup = await context.ProductGroups.Include(x => x.Products)
                                                      .FirstOrDefaultAsync(pg => pg.Products.Any(p => p.Id == productId), ct);

        if (productGroup is null)
            return ApplicationErrors.NotFound.Product;

        var result = productGroup.ApplyDiscount(productId, request.DiscountExpiresOn, request.DiscountPercentage);

        if (result.Failed)
            return result;


        await context.SaveAsync(ct);
        return Result.Success;
    }
}
