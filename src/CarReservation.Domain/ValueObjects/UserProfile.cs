namespace CarReservation.Domain.ValueObjects;

using CarReservation.Domain.Common;

public class UserProfile : ValueObject
{
    public string FirstName { get; }
    public string LastName { get; }
    public string? Phone { get; }
    public Address? Address { get; }
    public bool IsProfileComplete { get; }

    public UserProfile(string firstName, string lastName, string? phone = null, Address? address = null)
    {
        FirstName = firstName?.Trim() ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName?.Trim() ?? throw new ArgumentNullException(nameof(lastName));
        Phone = phone?.Trim();
        Address = address;

        ValidateNames();
        
        IsProfileComplete = !string.IsNullOrWhiteSpace(FirstName) && 
                           !string.IsNullOrWhiteSpace(LastName);
    }

    public string FullName => $"{FirstName} {LastName}";

    public UserProfile UpdatePhone(string? phone)
    {
        return new UserProfile(FirstName, LastName, phone?.Trim(), Address);
    }

    public UserProfile UpdateNames(string firstName, string lastName)
    {
        return new UserProfile(firstName, lastName, Phone, Address);
    }

    public UserProfile UpdateAddress(Address? address)
    {
        return new UserProfile(FirstName, LastName, Phone, address);
    }

    public UserProfile UpdateDepartment(string? department)
    {
        return new UserProfile(FirstName, LastName, Phone, Address);
    }

    public UserProfile UpdateAll(string firstName, string lastName, string? phone, Address? address)
    {
        return new UserProfile(firstName, lastName, phone, address);
    }

    private void ValidateNames()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
            throw new ArgumentException("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(LastName))
            throw new ArgumentException("Last name cannot be empty");

        if (FirstName.Length > 50)
            throw new ArgumentException("First name is too long");

        if (LastName.Length > 50)
            throw new ArgumentException("Last name is too long");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return Phone ?? string.Empty;
        yield return Address?.ToString() ?? string.Empty;
    }

    public override string ToString() => FullName;
}
