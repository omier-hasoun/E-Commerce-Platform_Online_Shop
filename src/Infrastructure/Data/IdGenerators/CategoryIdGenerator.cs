using Domain.Categories;
using App = Application.Common.Abstractions;

namespace Infrastructure.Data.IdGenerators;
internal sealed class CategoryIdGenerator([FromKeyedServices("Snowflake")] IPrimitiveTypeIdGenerator<long> Generator) : App.IIdGenerator<CategoryId>
{
    public CategoryId NewId()
    {
        return new CategoryId(Generator.Generate());
    }
}
