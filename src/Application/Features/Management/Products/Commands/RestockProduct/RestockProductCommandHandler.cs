
using Domain.Inventories;

namespace Application.Features.Management.Products.Commands.RestockProduct;

internal sealed class RestockProductCommandHandler(IAppDbContext context) : IRequestHandler<RestockProductCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(RestockProductCommand request, CancellationToken ct)
    {
        var inventory = await context.Inventories.FindAsync(request.ProductId, request.WarehouseId);

        if(inventory is null)
        {
            var result = Inventory.Create(request.WarehouseId, request.ProductId, request.StockQuantity);

            if (result.Failed)
            {
                return result.Errors;
            }
            inventory = result.Value;

            context.Inventories.Add(inventory);
        }
        else
        {
            var result = inventory.Restock(request.StockQuantity);

            if (result.Failed)
            {
                return result.Errors;
            }
        }

        await context.SaveAsync(ct);

        return Result.Updated;
    }
}
