
using IdGen;
using App = Application.Common.Abstractions;

namespace Infrastructure.Data.IdGenerators.Primitives;

internal sealed class SnowflakeGenerator(IdGen.IIdGenerator<long> _gen): IPrimitiveTypeIdGenerator<long>, App.IIdGenerator<long>
{
    public long Generate()
    {
        return _gen.CreateId();
    }

    public long NewId()
    {
        return _gen.CreateId();
    }
}
