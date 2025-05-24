namespace CarReservation.Domain.ValueObjects;

using CarReservation.Domain.Common;

public class CarId : ValueObject
{
    public Guid Value { get; }

    private CarId(Guid value)
    {
        Value = value;
    }

    public static CarId Create() => new(Guid.NewGuid());
    public static CarId From(Guid value) => new(value);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToString();
}
