

namespace Domain.Brands;

public sealed class Brand : AggregateRoot<BrandId>, IHasCreationTime
{
    private Brand(BrandId id, string name, string? description, string companyName, string slug, string? logoUrl, DateTime createdAt, bool isActive)
        : base(id)
    {
        Name = name;
        Description = description;
        CompanyName = companyName;
        Slug = slug;
        LogoUrl = logoUrl;
        CreatedAt = createdAt;
        IsActive = isActive;
    }

    public static Result<Brand> Create(BrandId id, string name, string? description, string companyName, string slug, string? logoUrl)
    {
        var validationResult = Result.ValidateAll(() => ValidateCompanyName(companyName),
                                                  () => ValidateLogoUrl(logoUrl),
                                                  () => ValidateName(name),
                                                  () => ValidateSlug(slug),
                                                  () => ValidateDescription(description)
                                                );
        return new Brand(id, name, description, companyName, slug, logoUrl, DateTime.UtcNow, isActive: true);
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string CompanyName { get; private set; }
    public string? LogoUrl { get; private set; }
    public string Slug { get; private set; }
    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; set; }


    public Result<Success> SetAsActivated()
    {
        IsActive = true;
        return Result.Success;
    }
    public Result<Success> SetAsDeactivated()
    {
        IsActive = false;
        return Result.Success;
    }

    private static Result<Success> ValidateName(string name) =>
        !string.IsNullOrWhiteSpace(name) ? Result.Success : DomainErrors.Brands.NameInvalid;
    private static Result<Success> ValidateDescription(string? description) =>
     description == null || !string.IsNullOrWhiteSpace(description) ? Result.Success : DomainErrors.Brands.DescriptionInvalid;
    private static Result<Success> ValidateCompanyName(string companyName)
        => !string.IsNullOrWhiteSpace(companyName) ? Result.Success : DomainErrors.Brands.CompanyNameInvalid;
    private static Result<Success> ValidateSlug(string skuName)
        => !string.IsNullOrWhiteSpace(skuName) ? Result.Success : DomainErrors.Brands.SlugInvalid;
    private static Result<Success> ValidateLogoUrl(string? logoUrl) =>
    logoUrl == null || !string.IsNullOrWhiteSpace(logoUrl) ? Result.Success : DomainErrors.Brands.LogoUrlInvalid;
}
