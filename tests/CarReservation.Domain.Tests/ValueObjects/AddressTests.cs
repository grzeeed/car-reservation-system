namespace CarReservation.Domain.Tests.ValueObjects;

using CarReservation.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

public class AddressTests
{
    [Fact]
    public void CreateAddress_WithValidData_ShouldCreateSuccessfully()
    {
        // Arrange
        var street = "123 Main Street";
        var city = "New York";
        var state = "NY";
        var postalCode = "10001";
        var country = "USA";

        // Act
        var address = new Address(street, city, state, postalCode, country);

        // Assert
        address.Street.Should().Be(street);
        address.City.Should().Be(city);
        address.State.Should().Be(state);
        address.PostalCode.Should().Be(postalCode);
        address.Country.Should().Be(country);
        address.FullAddress.Should().Be("123 Main Street, New York, NY 10001, USA");
    }

    [Fact]
    public void CreateAddress_WithEmptyStreet_ShouldThrowException()
    {
        // Arrange & Act & Assert
        var act = () => new Address("", "New York", "NY", "10001", "USA");
        act.Should().Throw<ArgumentException>()
           .WithMessage("Street cannot be empty*");
    }

    [Fact]
    public void CreateAddress_WithEmptyCity_ShouldThrowException()
    {
        // Arrange & Act & Assert
        var act = () => new Address("123 Main Street", "", "NY", "10001", "USA");
        act.Should().Throw<ArgumentException>()
           .WithMessage("City cannot be empty*");
    }

    [Fact]
    public void CreateAddress_WithTooLongStreet_ShouldThrowException()
    {
        // Arrange
        var longStreet = new string('A', 201);

        // Act & Assert
        var act = () => new Address(longStreet, "New York", "NY", "10001", "USA");
        act.Should().Throw<ArgumentException>()
           .WithMessage("Street address is too long*");
    }

    [Fact]
    public void CreateAddress_WithTooShortPostalCode_ShouldThrowException()
    {
        // Arrange & Act & Assert
        var act = () => new Address("123 Main Street", "New York", "NY", "12", "USA");
        act.Should().Throw<ArgumentException>()
           .WithMessage("Postal code is too short*");
    }

    [Fact]
    public void UpdateStreet_WithNewStreet_ShouldReturnNewAddress()
    {
        // Arrange
        var originalAddress = new Address("123 Main Street", "New York", "NY", "10001", "USA");
        var newStreet = "456 Oak Avenue";

        // Act
        var updatedAddress = originalAddress.UpdateStreet(newStreet);

        // Assert
        updatedAddress.Street.Should().Be(newStreet);
        updatedAddress.City.Should().Be(originalAddress.City);
        updatedAddress.State.Should().Be(originalAddress.State);
        updatedAddress.PostalCode.Should().Be(originalAddress.PostalCode);
        updatedAddress.Country.Should().Be(originalAddress.Country);
    }

    [Fact]
    public void UpdateCity_WithNewCity_ShouldReturnNewAddress()
    {
        // Arrange
        var originalAddress = new Address("123 Main Street", "New York", "NY", "10001", "USA");
        var newCity = "Los Angeles";

        // Act
        var updatedAddress = originalAddress.UpdateCity(newCity);

        // Assert
        updatedAddress.City.Should().Be(newCity);
        updatedAddress.Street.Should().Be(originalAddress.Street);
    }

    [Fact]
    public void IsInCountry_WithMatchingCountry_ShouldReturnTrue()
    {
        // Arrange
        var address = new Address("123 Main Street", "New York", "NY", "10001", "USA");

        // Act
        var result = address.IsInCountry("USA");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInCountry_WithDifferentCountry_ShouldReturnFalse()
    {
        // Arrange
        var address = new Address("123 Main Street", "New York", "NY", "10001", "USA");

        // Act
        var result = address.IsInCountry("Canada");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsInCountry_WithCaseInsensitiveMatch_ShouldReturnTrue()
    {
        // Arrange
        var address = new Address("123 Main Street", "New York", "NY", "10001", "USA");

        // Act
        var result = address.IsInCountry("usa");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInState_WithMatchingState_ShouldReturnTrue()
    {
        // Arrange
        var address = new Address("123 Main Street", "New York", "NY", "10001", "USA");

        // Act
        var result = address.IsInState("NY");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInCity_WithMatchingCity_ShouldReturnTrue()
    {
        // Arrange
        var address = new Address("123 Main Street", "New York", "NY", "10001", "USA");

        // Act
        var result = address.IsInCity("New York");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void TwoAddressesWithSameData_ShouldBeEqual()
    {
        // Arrange
        var address1 = new Address("123 Main Street", "New York", "NY", "10001", "USA");
        var address2 = new Address("123 Main Street", "New York", "NY", "10001", "USA");

        // Act & Assert
        address1.Should().Be(address2);
        address1.GetHashCode().Should().Be(address2.GetHashCode());
    }

    [Fact]
    public void TwoAddressesWithDifferentData_ShouldNotBeEqual()
    {
        // Arrange
        var address1 = new Address("123 Main Street", "New York", "NY", "10001", "USA");
        var address2 = new Address("456 Oak Avenue", "New York", "NY", "10001", "USA");

        // Act & Assert
        address1.Should().NotBe(address2);
    }

    [Fact]
    public void ToString_ShouldReturnFullAddress()
    {
        // Arrange
        var address = new Address("123 Main Street", "New York", "NY", "10001", "USA");

        // Act
        var result = address.ToString();

        // Assert
        result.Should().Be("123 Main Street, New York, NY 10001, USA");
    }
}
