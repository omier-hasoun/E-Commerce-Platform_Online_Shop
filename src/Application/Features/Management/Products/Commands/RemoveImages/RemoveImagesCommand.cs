
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.RemoveImages;

public sealed record RemoveImagesCommand( long ProductId, List<string> FileNames) : IRequest<Result<Deleted>>
{

    internal ProductId ParsedProductId =>
    new(ProductId);
}
