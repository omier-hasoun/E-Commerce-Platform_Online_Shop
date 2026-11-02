
namespace Application.Features.Management.Products.Commands.ArchiveProduct;

internal sealed class ArchiveProductCommandHandler(IAppDbContext context) : IRequestHandler<ArchiveProductCommand, Result<Success>>
{
    public async Task<Result<Success>> Handle(ArchiveProductCommand command, CancellationToken ct)
    {
        var productId = command.ParsedProductId;

        var productGroup = await context.ProductGroups.Include(x => x.Products)
                                                .ThenInclude(x => x.Inventory)
                                                .FirstOrDefaultAsync(pg => pg.Products.Any(p => p.Id == productId), ct);

        if (productGroup is null)
        {
            return ApplicationErrors.NotFound.ProductGroup;
        }

        var res = productGroup.ArchiveGroup();

        if (res.Failed)
            return res.Errors;

        if(command.ResetProductsStock)
        {
            foreach (var product in productGroup.Products)
            {
                product.Inventory.ResetStock();
            }
        }
        await context.SaveAsync(ct);

        return Result.Success;
    }

}
