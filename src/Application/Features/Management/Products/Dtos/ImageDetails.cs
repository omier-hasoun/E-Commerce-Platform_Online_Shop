namespace Application.Features.Management.Products.Dtos;

public sealed class ImageDetails
{
    // If it already exists on the server, supply its ID. Otherwise, null.
    public long? ExistingId { get; set; }

    // If it's a new image, this points to the index in the uploaded IFormFile collection
    public int? NewFileIndex { get; set; }

    // The sequence/sort order defined by the user drag-and-drop or UI position
    required public byte SortOrder { get; set; }
}
