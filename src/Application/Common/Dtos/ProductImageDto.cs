using Domain.ProductGroups.ValueObjects;

namespace Application.Common.Dtos;

public sealed record ProductImageDto
{
    public long Id { get; init; }
    public string Url { get; init; }
    public byte SortOrder { get; init; }

    public ProductImageDto(long id, string image, byte SortOrder)
    {
        this.Id = id;
        this.Url = image;
        this.SortOrder = SortOrder;
    }


    internal ProductImageDto(ProductImage image) : this(image.Id, image.FileName, image.SortOrder)
    {

    }

    public static List<ProductImageDto> FromProductImages(List<ProductImage> images)
    {
        List<ProductImageDto> imagesDto = new(images.Count);

        images.ForEach(image =>
        {
            imagesDto.Add(new ProductImageDto(image));
        });

        return imagesDto;
    }

}
