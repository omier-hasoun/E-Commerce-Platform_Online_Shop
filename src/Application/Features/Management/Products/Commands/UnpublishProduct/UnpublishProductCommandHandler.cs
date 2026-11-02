
namespace Application.Features.Management.Products.Commands.UnpublishProduct;

internal sealed class UnpublishProductCommandHandler(IAppDbContext context) : IRequestHandler<UnpublishProductCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(UnpublishProductCommand command, CancellationToken ct)
    {
        var productId = command.ParsedProductId;

        var productGroup = await context.ProductGroups.Include(x => x.Products)
                                                      .FirstOrDefaultAsync(pg => pg.Products.Any(p => p.Id == productId), ct);

        if (productGroup is null)
        {
            return ApplicationErrors.NotFound.ProductGroup;
        }

        var res = productGroup.UnpublishProduct(productId);

        if (res.Failed)
            return res.Errors;

        await context.SaveAsync(ct);

        return Result.Success;
    }
}
