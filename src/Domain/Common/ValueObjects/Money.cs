
using System.Text.Json.Serialization;

namespace Domain.Common.ValueObjects;

public sealed record Money
{
    public const int MaxValue = 1_000_0000;
    private const int _maxCentsvalue = 1_000_000_000;

    public static readonly Money Zero = new Money() { Value = 0};

    internal Money()
    {

    }

    [JsonConstructor]
    public Money(decimal value)
    {
        Value = value;
    }


    public static Money Create(decimal value)
    {
        if(ValHelper.IsOutOfRange(value, 0, MaxValue))
        {
            throw new ArgumentException("money cannot be less than zero");
        }

        return new Money()
        {
            Value = value
        };
    }

    public static Money Create(long value)
    {
        return Create((decimal)value);
    }


    public static Money FromCents(long value)
    {
        if (ValHelper.IsOutOfRange(value, 0, _maxCentsvalue))
        {
            throw new ArgumentException("money cannot be less than zero or more than 1billion");
        }

        decimal decimalValue = Math.Round(value / 100m, 2, MidpointRounding.AwayFromZero);

        return new Money()
        {
            Value = decimalValue
        };
    }
    public long ToCents() => ToCents(Value);

    public static long ToCents(decimal value)
    {
        return (long)Math.Round(
            value * 100m,
            0,
            MidpointRounding.AwayFromZero
        );
    }

    public override string ToString()
    {
        return Value.ToString("N2");
    }

    public decimal Value { get; internal init; }
    public static bool operator <(Money left, Money right) =>
        left is not null && right is not null && left.Value < right.Value;

    public static bool operator >(Money left, Money right) =>
        left is not null && right is not null && left.Value > right.Value;

    public static bool operator <=(Money left, Money right) =>
        left is not null && right is not null && left.Value <= right.Value;

    public static bool operator >=(Money left, Money right) =>
        left is not null && right is not null && left.Value >= right.Value;

    public static Money operator *(Money left, Money right)
    {
        if(left is null || right is null)
        {
            return Create(0);
        }


      return Create(left.Value * right.Value);
    }

    public static Money operator +(Money left, Money right)
    {
        if (left is null || right is null)
        {
            throw new ArgumentNullException();
        }


        return Create(left.Value + right.Value);
    }
}
