

namespace Application.Features.Public.Products.Queries.SearchProductSuggestions;

public sealed class SearchProductSuggestionsQueryValidator : AbstractValidator<SearchProductSuggestionsQuery>
{
    public SearchProductSuggestionsQueryValidator()
    {
        RuleFor(x => x.SearchTerm)
        .NotEmpty()
        .MaximumLength(40);

    }
}
