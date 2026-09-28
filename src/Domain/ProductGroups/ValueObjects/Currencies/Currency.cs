
namespace Domain.Currencies;

public sealed class Currency : IEntity
{
    public Currency()
    {
        
    }
    public string Name { get; init; } = null!;

    public string Code { get; init; } = null!;

    public string Symbol { get; init; } = null!;


}
