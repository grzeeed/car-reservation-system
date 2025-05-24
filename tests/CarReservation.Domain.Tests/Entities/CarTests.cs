namespace CarReservation.Domain.Tests.Entities;

using CarReservation.Domain.Entities;
using CarReservation.Domain.Enums;
using CarReservation.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

public class CarTests
{
    [Fact]
    public void CreateCar_WithValidData_ShouldCreateCarSuccessfully()
    {
        // Arrange
        var carId = CarId.Create();
        var pricePerDay = new Money(100, "USD");
        var location = new Location("New York", "123 Main St", 40.7128, -74.0060);

        // Act
        var car = new Car(carId, "Toyota", "Camry", "ABC123", CarType.Sedan, pricePerDay, location);

        // Assert
        car.Id.Should().Be(carId);
        car.Brand.Should().Be("Toyota");
        car.Model.Should().Be("Camry");
        car.LicensePlate.Should().Be("ABC123");
        car.Type.Should().Be(CarType.Sedan);
        car.Status.Should().Be(CarStatus.Available);
        car.DomainEvents.Should().HaveCount(1);
    }

    [Fact]
    public void Reserve_WhenCarIsAvailable_ShouldReserveSuccessfully()
    {
        // Arrange
        var car = CreateTestCar();
        var customerId = CustomerId.Create();
        var period = new DateRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));

        // Act
        var result = car.Reserve(customerId, period);

        // Assert
        result.IsSuccess.Should().BeTrue();
        car.Status.Should().Be(CarStatus.Reserved);
        car.Reservations.Should().HaveCount(1);
        car.DomainEvents.Should().HaveCount(2); // CarCreated + CarReserved
    }

    [Fact]
    public void Reserve_WhenCarIsNotAvailable_ShouldReturnFailure()
    {
        // Arrange
        var car = CreateTestCar();
        car.SetMaintenance();
        var customerId = CustomerId.Create();
        var period = new DateRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));

        // Act
        var result = car.Reserve(customerId, period);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Car is not available for reservation");
    }

    [Fact]
    public void Reserve_WhenPeriodOverlaps_ShouldReturnFailure()
    {
        // Arrange
        var car = CreateTestCar();
        var customerId1 = CustomerId.Create();
        var customerId2 = CustomerId.Create();
        var period1 = new DateRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(5));
        var period2 = new DateRange(DateTime.Today.AddDays(3), DateTime.Today.AddDays(7));

        car.Reserve(customerId1, period1);

        // Act
        var result = car.Reserve(customerId2, period2);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Car is not available for reservation");
        car.Reservations.Should().HaveCount(1);
    }

    [Fact]
    public void SetMaintenance_WhenCarHasNoActiveReservations_ShouldSucceed()
    {
        // Arrange
        var car = CreateTestCar();

        // Act
        var result = car.SetMaintenance();

        // Assert
        result.IsSuccess.Should().BeTrue();
        car.Status.Should().Be(CarStatus.InMaintenance);
    }

    [Fact]
    public void SetMaintenance_WhenCarHasActiveReservations_ShouldFail()
    {
        // Arrange
        var car = CreateTestCar();
        var customerId = CustomerId.Create();
        var period = new DateRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));
        car.Reserve(customerId, period);

        // Act
        var result = car.SetMaintenance();

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Cannot set car to maintenance while it has active reservations");
    }

    [Fact]
    public void IsAvailableForPeriod_WhenCarIsAvailableAndPeriodIsFree_ShouldReturnTrue()
    {
        // Arrange
        var car = CreateTestCar();
        var period = new DateRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));

        // Act
        var result = car.IsAvailableForPeriod(period);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsAvailableForPeriod_WhenCarIsInMaintenance_ShouldReturnFalse()
    {
        // Arrange
        var car = CreateTestCar();
        car.SetMaintenance();
        var period = new DateRange(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));

        // Act
        var result = car.IsAvailableForPeriod(period);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void UpdatePricing_WithValidPrice_ShouldUpdateSuccessfully()
    {
        // Arrange
        var car = CreateTestCar();
        var newPrice = new Money(150, "USD");

        // Act
        var result = car.UpdatePricing(newPrice);

        // Assert
        result.IsSuccess.Should().BeTrue();
        car.PricePerDay.Should().Be(newPrice);
    }

    [Fact]
    public void UpdatePricing_WithZeroPrice_ShouldFail()
    {
        // Arrange
        var car = CreateTestCar();
        var invalidPrice = new Money(0, "USD");

        // Act
        var result = car.UpdatePricing(invalidPrice);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Price must be greater than zero");
    }

    private static Car CreateTestCar()
    {
        var carId = CarId.Create();
        var pricePerDay = new Money(100, "USD");
        var location = new Location("New York", "123 Main St", 40.7128, -74.0060);
        return new Car(carId, "Toyota", "Camry", "ABC123", CarType.Sedan, pricePerDay, location);
    }
}
