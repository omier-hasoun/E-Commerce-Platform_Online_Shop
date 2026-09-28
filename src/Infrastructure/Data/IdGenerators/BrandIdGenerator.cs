
using Domain.Brands;

namespace Infrastructure.Data.IdGenerators;
internal sealed class BrandIdGenerator([FromKeyedServices("GuidV7")] IPrimitiveTypeIdGenerator<Guid> Generator) : IIdGenerator<BrandId>
{
    public BrandId NewId()
    {
        return new BrandId(Generator.Generate());
    }
}
