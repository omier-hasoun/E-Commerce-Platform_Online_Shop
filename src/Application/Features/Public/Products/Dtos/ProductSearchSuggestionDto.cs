using Domain.ProductGroups.Products;

namespace Application.Features.Public.Products.Dtos;

public record ProductSearchSuggestionDto
{

    public string NameSuggestion { get; }
    public ProductSearchSuggestionDto(string name)
    {
            this.NameSuggestion = name;

    }
};
