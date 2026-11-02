
namespace Application.Features.Public.Products.Queries.SearchProductSuggestions;

public sealed record SearchProductSuggestionsQuery(string SearchTerm) : IRequest<Result<List<string>>>;
