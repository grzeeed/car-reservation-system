namespace CarReservation.Domain.ValueObjects;

using CarReservation.Domain.Common;

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string PostalCode { get; }
    public string Country { get; }

    public Address(string street, string city, string state, string postalCode, string country)
    {
        Street = street?.Trim() ?? throw new ArgumentNullException(nameof(street));
        City = city?.Trim() ?? throw new ArgumentNullException(nameof(city));
        State = state?.Trim() ?? throw new ArgumentNullException(nameof(state));
        PostalCode = postalCode?.Trim() ?? throw new ArgumentNullException(nameof(postalCode));
        Country = country?.Trim() ?? throw new ArgumentNullException(nameof(country));

        ValidateAddress();
    }

    public string FullAddress => $"{Street}, {City}, {State} {PostalCode}, {Country}";

    public static Address Create(string street, string city, string state, string postalCode, string country)
    {
        return new Address(street, city, state, postalCode, country);
    }

    public Address UpdateStreet(string newStreet)
    {
        return new Address(newStreet, City, State, PostalCode, Country);
    }

    public Address UpdateCity(string newCity)
    {
        return new Address(Street, newCity, State, PostalCode, Country);
    }

    public Address UpdateState(string newState)
    {
        return new Address(Street, City, newState, PostalCode, Country);
    }

    public Address UpdatePostalCode(string newPostalCode)
    {
        return new Address(Street, City, State, newPostalCode, Country);
    }

    public Address UpdateCountry(string newCountry)
    {
        return new Address(Street, City, State, PostalCode, newCountry);
    }

    public bool IsInCountry(string country)
    {
        return string.Equals(Country, country?.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInState(string state)
    {
        return string.Equals(State, state?.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInCity(string city)
    {
        return string.Equals(City, city?.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    private void ValidateAddress()
    {
        if (string.IsNullOrWhiteSpace(Street))
            throw new ArgumentException("Street cannot be empty", nameof(Street));

        if (string.IsNullOrWhiteSpace(City))
            throw new ArgumentException("City cannot be empty", nameof(City));

        if (string.IsNullOrWhiteSpace(State))
            throw new ArgumentException("State cannot be empty", nameof(State));

        if (string.IsNullOrWhiteSpace(PostalCode))
            throw new ArgumentException("Postal code cannot be empty", nameof(PostalCode));

        if (string.IsNullOrWhiteSpace(Country))
            throw new ArgumentException("Country cannot be empty", nameof(Country));

        // Validation of lengths
        if (Street.Length > 200)
            throw new ArgumentException("Street address is too long", nameof(Street));

        if (City.Length > 100)
            throw new ArgumentException("City name is too long", nameof(City));

        if (State.Length > 100)
            throw new ArgumentException("State name is too long", nameof(State));

        if (PostalCode.Length > 20)
            throw new ArgumentException("Postal code is too long", nameof(PostalCode));

        if (Country.Length > 100)
            throw new ArgumentException("Country name is too long", nameof(Country));

        // Basic postal code validation (can be enhanced per country)
        if (PostalCode.Length < 3)
            throw new ArgumentException("Postal code is too short", nameof(PostalCode));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return PostalCode;
        yield return Country;
    }

    public override string ToString() => FullAddress;
}
