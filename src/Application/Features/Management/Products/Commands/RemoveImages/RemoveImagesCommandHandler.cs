
namespace Application.Features.Management.Products.Commands.RemoveImages;

internal sealed class RemoveImagesCommandHandler(IAppDbContext context, IImageStorageService store) : IRequestHandler<RemoveImagesCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(RemoveImagesCommand request, CancellationToken ct)
    {
        var productGroup = await context.ProductGroups.Include(x => x.Products)
                                                      .FirstOrDefaultAsync(pg => pg.Products.Any(p => p.Id == request.ParsedProductId), ct);

        if (productGroup is null || !productGroup.ProductExists(request.ParsedProductId))
            return ApplicationErrors.NotFound.Product;

        var result = productGroup.RemoveProductImages(request.ParsedProductId, request.FileNames);

        if (result.Failed)
            return result;

        var physicalDeleteResult = store.DeleteAll(request.FileNames);
        if (physicalDeleteResult.Failed)
            return physicalDeleteResult.Errors;

        await context.SaveAsync(ct);

        return Result.Deleted;
    }
}
