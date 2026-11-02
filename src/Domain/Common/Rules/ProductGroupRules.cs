namespace Domain.Common.Rules;

public static class ProductGroupRules
{
    // Name length limits
    public const byte MinTitleLength = 5;
    public const byte MaxTitleLength = 60;

    // Description length limits
    public const short MinDescriptionLength = 5;
    public const short MaxDescriptionLength = 2000;

    // ProductGroup product count limits
    public const byte MinNumberOfVariants = 1;
    public const byte MaxNumberOfVariants = 15;

    // ProductGroup Specifications count limits
    public const byte MinNumberOfSpecifications = 0;
    public const byte MaxNumberOfSpecifications = 50;

    // Rating limits. 1 to 5 stars or 0 if no one rated it
    public const byte MinAverageRatingValue = 0;
    public const byte MaxAverageRatingValue = 5;

}
