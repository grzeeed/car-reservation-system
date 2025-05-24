namespace CarReservation.Domain.ValueObjects;

using CarReservation.Domain.Common;

public class Location : ValueObject
{
    public string City { get; }
    public string Address { get; }
    public double Latitude { get; }
    public double Longitude { get; }

    public Location(string city, string address, double latitude, double longitude)
    {
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City cannot be empty", nameof(city));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be empty", nameof(address));

        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Invalid latitude", nameof(latitude));

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Invalid longitude", nameof(longitude));

        City = city;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return City;
        yield return Address;
        yield return Latitude;
        yield return Longitude;
    }

    public override string ToString() => $"{Address}, {City} ({Latitude}, {Longitude})";
}
