namespace CarReservation.Application.Interfaces;

using CarReservation.Application.DTOs;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;
using CarReservation.Domain.Enums;

public interface ICarReadRepository
{
    Task<IEnumerable<Car>> GetAvailableCarsAsync(
        DateRange period, 
        CarType? type = null, 
        string? location = null, 
        CancellationToken cancellationToken = default);

    Task<CarAnalyticsDto?> GetCarAnalyticsAsync(
        Guid carId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Car>> GetCarsByLocationAsync(
        string city,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<Car>> GetCarsByStatusAsync(
        CarStatus status,
        CancellationToken cancellationToken = default);

    Task<int> GetTotalCarsCountAsync(CancellationToken cancellationToken = default);

    Task<int> GetAvailableCarsCountAsync(
        DateRange? period = null,
        CancellationToken cancellationToken = default);
}
