
namespace Domain.ProductGroups.ValueObjects;

public sealed record ProductImage
{

    private ProductImage() { }


    public long Id { get; private init; }
    public string FileName { get; private init; } = null!;

    public byte SortOrder { get; private init; }


    public static ProductImage Create(long id, string fileName, byte sortOrder)
    {
        return new ProductImage()
        {
            Id = id,
            FileName = fileName,
            SortOrder = sortOrder,
        };
    }

    internal ProductImage ChangeSortOrder(byte sortOrder)
    {
        return new ProductImage
        {
            Id = this.Id,
            FileName = this.FileName,
            SortOrder = sortOrder

        };
    }
}
