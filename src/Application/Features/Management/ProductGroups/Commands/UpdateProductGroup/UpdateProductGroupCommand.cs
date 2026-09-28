
namespace Application.Features.Management.ProductGroups.Commands.UpdateProductGroup;

public sealed record UpdateProductGroupCommand(long ProductGroupId, Guid? BrandId, long? CategoryId, string? Title, string? Description,
    bool? IsSerialized, Dictionary<string, string>? Specifications)
: IRequest<Result<Updated>>;

public static class UpdateProductCommandExtensions
{
    internal static bool HasChanges(this UpdateProductGroupCommand command)
    {
        if (command.Specifications is null && command.IsSerialized is null && command.Title is null && command.BrandId is null && command.CategoryId is null
            && command.Description is null)
            return false;

        return true;
    }
}
