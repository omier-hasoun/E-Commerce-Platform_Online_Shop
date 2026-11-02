
using Domain.Common.ValueObjects;

namespace Application.Features.Management.Products.Commands.CreateProduct;

public sealed class CreateProductGroupCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductGroupCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
                             .Length(ProductGroupRules.MinTitleLength, ProductGroupRules.MaxTitleLength);


        RuleFor(x => x.Description).NotEmpty()
                                   .Length(ProductGroupRules.MinDescriptionLength, ProductGroupRules.MaxDescriptionLength);

        RuleFor(x => x.Price).Must(x => x > 0 && x <= Money.MaxValue);

        RuleFor(x => x.Height).Must(x => x > 0);

        RuleFor(x => x.Weight).Must(x => x > 0);

        RuleFor(x => x.Length).Must(x => x > 0);

        RuleFor(x => x.Width).Must(x => x > 0);

        RuleFor(x => x.BrandId).NotEmpty();

        RuleFor(x => x.CategoryId).NotEmpty();

        // RuleFor(x => x.Specifications).NotEmpty()
        //                           .Must(x => x.Count <= ProductGroupRules.MaxNumberOfSpecifications)
        //                           .WithMessage($"Specifications cannot exceed {ProductGroupRules.MaxNumberOfSpecifications} values");

        // RuleForEach(x => x.Specifications)
        //                             .Must(kv => kv.Key.Length <= 50 && !string.IsNullOrWhiteSpace(kv.Key))
        //                             .WithMessage("Invalid key. It cannot be empty or too long");

        // RuleForEach(x => x.Specifications)
        //                         .Must(kv => kv.Value.Length <= 50 && string.IsNullOrWhiteSpace(kv.Value))
        //                         .WithMessage("Invalid value. It cannot be empty or too long");

        RuleFor(x => x.UploadedImages)
            .Must(x => x == null || x.Count == 0 || x.Count <= 10);

        RuleFor(x => x)
        .Must(x => x.UploadedImages == null || x.UploadedImages.Count == 0 || (x.UploadedImages.Count <= 30 && x.UploadedImages!.Count == x.ImagesDetails.Count))
        .WithMessage("UploadedImages must match ImagesDetails number Of objects");


        RuleForEach(x => x.UploadedImages)
                .NotNull();


    }
}
