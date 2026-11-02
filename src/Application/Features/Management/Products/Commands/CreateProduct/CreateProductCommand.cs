using Application.Features.Management.Products.Dtos;

namespace Application.Features.Management.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    Guid BrandId,
    long CategoryId,
    string Title,
    string Description,
    bool IsSerialized,
    decimal Price,
    int Width,
    int Height,
    int Length,
    int Weight,
    string Sku,
    string BarCode,
    string Slug,
    InventoryDto? Inventory = null,
    List<ImageDetails>? ImagesDetails = null,
    List<FileUploadDto>? UploadedImages = null,
    Dictionary<string, string>? Specifications = null) : IRequest<Result<long>>;
