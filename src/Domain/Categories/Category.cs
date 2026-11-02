
namespace Domain.Categories;

public class Category : AggregateRoot<CategoryId>
{
    private Category(CategoryId id, string name, CategoryId? parentCategoryId) : base(id)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
    }

    public static Result<Category> Create(CategoryId id, string name, CategoryId? parentCategoryId)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > CategoryRules.MaxCategoryNameLength)
            return DomainErrors.Categories.InvalidName;

        return new Category(id, name.Trim(), parentCategoryId);
    }

    public CategoryId? ParentCategoryId { get; private set; }
    public string Name { get; private set; } = null!;
}
