using Application.Features.Management.Products.Dtos;
using Domain.ProductGroups.Products;

namespace Application.Features.Management.Products.Commands.UpdateImages;

public sealed record UpdateImagesCommand : IRequest<Result<Success>>
{
    required public long ProductId { get; init; }
    required public List<FileUploadDto> NewFiles { get; init; }
    required public List<ImageDetails> Images { get; init; }


}
