
namespace Application.Features.Management.ProductGroups.Commands.CreateProductGroup;

internal sealed class CreateProductGroupCommandValidator : AbstractValidator<CreateProductGroupCommand>
{
    public CreateProductGroupCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
                             .Length(ProductGroupRules.MinTitleLength, ProductGroupRules.MaxTitleLength);


        RuleFor(x => x.Description).NotEmpty()
                                   .Length(ProductGroupRules.MinDescriptionLength, ProductGroupRules.MaxDescriptionLength);

        RuleFor(x => x.BrandId).NotEmpty();

        RuleFor(x => x.CategoryId).NotEmpty();

        RuleFor(x => x.Specifications).NotEmpty()
                                  .Must(x => x.Count <= ProductGroupRules.MaxNumberOfSpecifications)
                                  .WithMessage($"Specifications cannot exceed {ProductGroupRules.MaxNumberOfSpecifications} values");

        RuleForEach(x => x.Specifications)
                                    .Must(kv => kv.Key.Length <= 50 && !string.IsNullOrWhiteSpace(kv.Key))
                                    .WithMessage("Invalid key. It cannot be empty or too long");

        RuleForEach(x => x.Specifications)
                                .Must(kv => kv.Value.Length <= 50! && string.IsNullOrWhiteSpace(kv.Value))
                                .WithMessage("Invalid value. It cannot be empty or too long");




    }
}
