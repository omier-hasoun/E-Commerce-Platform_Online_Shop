

using Application.Features.Management.ProductGroups.Dtos;

namespace Application.Features.Management.ProductGroups.Queries.GetProductsGroupById;

internal sealed class GetProductsGroupByIdQueryHandler(IAppDbContext context) : IRequestHandler<GetProductsGroupByIdQuery, Result<ProductGroupDto>>
{
    public async Task<Result<ProductGroupDto>> Handle(GetProductsGroupByIdQuery query, CancellationToken ct)
    {
        var getProductQuery = context.ProductGroups.AsNoTracking()
                                              .Where(g => g.Id == query.ProductGroupId)
                                              .Join(
                                                    context.Users, pg => pg.LastModifiedBy, u => u.Id,
                                                    (pg, u) => new { pg, u }
                                              )
                                              .Select(
                                                  x => new ProductGroupDto(x.pg.Id, x.pg.Title, x.pg.Description, x.pg.Specifications,
                                                            x.pg.BrandId, x.pg.BrandName, x.pg.CategoryId, x.pg.CategoryName, x.pg.AverageRating, x.pg.LastModifiedAt,
                                                            x.pg.LastModifiedBy, x.u.UserName!,

                                                           x.pg.Products.Select(p => new ProductListItemDto(
                                                                                        p.Id, p.OriginalPrice, p.HasDiscount, p.DiscountPercentage,
                                                                                        p.DiscountPrice, p.DiscountExpiresOn, p.Status, p.Images.FirstOrDefault(),

                                                                                        new ProductInventoryDto(p.Inventory.WarehouseId,
                                                                                                                p.Inventory.Warehouse.Name,
                                                                                                                p.Inventory.StockQuantity)))
                                                                        .ToList())
                                              );


        var groupDto = await getProductQuery.FirstOrDefaultAsync(ct);

        if (groupDto is null)
            return ApplicationErrors.NotFound.ProductGroup;


        return groupDto;
    }
}
