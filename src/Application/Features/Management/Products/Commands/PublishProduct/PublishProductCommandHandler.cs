

namespace Application.Features.Management.Products.Commands.PublishProduct;

internal sealed class PublishProductCommandHandler(IAppDbContext context) : IRequestHandler<PublishProductCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(PublishProductCommand command, CancellationToken ct)
    {
        var productId = command.ParsedProductId;

        var productGroup = await context.ProductGroups.Include(x => x.MainProduct)
                                                      .ThenInclude(x => x!.Inventory)
                                                      .FirstOrDefaultAsync(pg => pg.Products.Any(p => p.Id == productId), ct);

        if (productGroup is null)
        {
            return ApplicationErrors.NotFound.ProductGroup;
        }

        var res = productGroup.PublishProduct(productId);

        if (res.Failed)
            return res.Errors;

        await context.SaveAsync(ct);

        return Result.Success;
    }
}
