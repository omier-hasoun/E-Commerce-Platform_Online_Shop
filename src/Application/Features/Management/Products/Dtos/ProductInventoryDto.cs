
using Domain.Warehouses;

namespace Application.Features.Management.Products.Dtos;

public sealed record ProductInventoryDto
{
    public ProductInventoryDto(WarehouseId id, string warehouseName, int stockQuantity)
    {
        this.Id = id.Value;
        this.WarehouseName = warehouseName;
        this.StockQuantity = stockQuantity;
    }

    public long Id { get; }
    public string WarehouseName { get; }
    public int StockQuantity { get; }
}

