namespace CarReservation.Domain.ValueObjects;

using CarReservation.Domain.Common;

public class CustomerId : ValueObject
{
    public Guid Value { get; }

    private CustomerId(Guid value)
    {
        Value = value;
    }

    public static CustomerId Create() => new(Guid.NewGuid());
    public static CustomerId From(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
