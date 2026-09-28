namespace Application.Features.Management.ProductGroups.Commands.CreateProductGroup;

public sealed record CreateProductGroupCommand(
Guid BrandId, long CategoryId, string Title, string Description, bool IsSerialized, Dictionary<string, string> Specifications
) : IRequest<Result<long>>;
