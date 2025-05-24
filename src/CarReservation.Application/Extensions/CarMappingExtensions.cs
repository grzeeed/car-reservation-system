using CarReservation.Application.DTOs;
using CarReservation.Domain.Entities;
using CarReservation.Domain.Enums;
using CarReservation.Domain.ValueObjects;

namespace CarReservation.Application.Extensions;

public static class CarMappingExtensions
{
    public static CarDto ToDto(this Car car)
    {
        return new CarDto(
            Id: car.Id.Value,
            Brand: car.Brand,
            Model: car.Model,
            LicensePlate: car.LicensePlate,
            CarType: car.Type.ToString(),
            PricePerDay: car.PricePerDay.Amount,
            Currency: car.PricePerDay.Currency,
            Status: car.Status.ToString(),
            CurrentLocation: car.CurrentLocation.ToDto(),
            Reservations: car.Reservations.Select(r => r.ToDto()).ToList()
        );
    }

    public static CarSummaryDto ToSummaryDto(this Car car)
    {
        return new CarSummaryDto(
            Id: car.Id.Value,
            Brand: car.Brand,
            Model: car.Model,
            LicensePlate: car.LicensePlate,
            CarType: car.Type.ToString(),
            PricePerDay: car.PricePerDay.Amount,
            Currency: car.PricePerDay.Currency,
            Status: car.Status.ToString(),
            CurrentLocation: car.CurrentLocation.ToDto()
        );
    }

    public static Car ToEntity(this CreateCarDto dto)
    {
        if (!Enum.TryParse<CarType>(dto.CarType, out var carType))
            throw new ArgumentException($"Invalid car type: {dto.CarType}");

        var carId = CarId.Create();
        var money = new Money(dto.PricePerDay, dto.Currency);
        var location = dto.Location.ToEntity();

        return new Car(
            id: carId,
            brand: dto.Brand,
            model: dto.Model,
            licensePlate: dto.LicensePlate,
            type: carType,
            pricePerDay: money,
            location: location
        );
    }

    public static List<CarDto> ToDto(this IEnumerable<Car> cars)
    {
        return cars.Select(car => car.ToDto()).ToList();
    }

    public static List<CarSummaryDto> ToSummaryDto(this IEnumerable<Car> cars)
    {
        return cars.Select(car => car.ToSummaryDto()).ToList();
    }
}
