

namespace Application.Features.Public.Products.Queries.SearchProductSuggestions;

internal sealed class SearchProductSuggestionsQueryHandler(IAppDbContext db) : IRequestHandler<SearchProductSuggestionsQuery, Result<List<string>>>
{
    public async Task<Result<List<string>>> Handle(SearchProductSuggestionsQuery request, CancellationToken ct)
    {

        var suggestions = db.ProductGroups.ApplySearchTextFilter(request.SearchTerm)
                                          .ApplyFilterByPublishedProducts()
                                          .OrderByDescending(x => x.NormalizedTitle)
                                          .Take(5)
                                          .Select(x => x.Title)
                                          .ToList();

        return suggestions;
    }
}
