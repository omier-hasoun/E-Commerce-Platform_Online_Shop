namespace Api.Requests;

public sealed record UpdateProductGroupRequest
{
    public Guid? BrandId { get; init; }

    public long? CategoryId { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public bool? IsSerialized { get; init; }

    public Dictionary<string, string>? Specifications { get; init; }

    public UpdateProductGroupRequest()
    {
    }

}
