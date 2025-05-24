namespace CarReservation.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using CarReservation.Application.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;
using CarReservation.Domain.Enums;

public class CarReadRepository : ICarReadRepository
{
    private readonly CarReservationDbContext _context;

    public CarReadRepository(CarReservationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Car>> GetAvailableCarsAsync(
        DateRange period, 
        CarType? type = null, 
        string? location = null, 
        CancellationToken cancellationToken = default)
    {
        var query = _context.Cars
            .Include(c => c.Reservations)
            .Where(c => c.Status == CarStatus.Available);

        // Filter by type if specified
        if (type.HasValue)
        {
            query = query.Where(c => c.Type == type.Value);
        }

        // Filter by location if specified
        if (!string.IsNullOrWhiteSpace(location))
        {
            query = query.Where(c => c.CurrentLocation.City.Contains(location));
        }

        var cars = await query.ToListAsync(cancellationToken);

        // Filter out cars that have overlapping reservations
        return cars.Where(car => !car.Reservations.Any(r => 
            r.Status == ReservationStatus.Active && 
            r.Period.OverlapsWith(period)));
    }

    public async Task<CarAnalyticsDto?> GetCarAnalyticsAsync(
        Guid carId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var car = await _context.Cars
            .Include(c => c.Reservations)
            .FirstOrDefaultAsync(c => c.Id == CarId.From(carId), cancellationToken);

        if (car == null)
            return null;

        var period = new DateRange(startDate, endDate);
        var relevantReservations = car.Reservations
            .Where(r => r.Period.OverlapsWith(period))
            .ToList();

        var totalReservations = relevantReservations.Count;
        var completedReservations = relevantReservations.Count(r => r.Status == ReservationStatus.Completed);
        var cancelledReservations = relevantReservations.Count(r => r.Status == ReservationStatus.Cancelled);
        var activeReservations = relevantReservations.Count(r => r.Status == ReservationStatus.Active);

        var averageReservationDuration = relevantReservations.Any() 
            ? relevantReservations.Average(r => r.Period.Days) 
            : 0;

        var cancellationRate = totalReservations > 0 
            ? (double)cancelledReservations / totalReservations * 100 
            : 0;

        var totalRevenue = car.CalculateRevenueForPeriod(period);
        var averageRevenuePerReservation = completedReservations > 0 
            ? totalRevenue.Amount / completedReservations 
            : 0;

        var daysInPeriod = period.Days;
        var daysReserved = relevantReservations.Sum(r => r.Period.Days);
        var utilizationRate = daysInPeriod > 0 ? (double)daysReserved / daysInPeriod * 100 : 0;

        return new CarAnalyticsDto(
            carId,
            car.Brand,
            car.Model,
            car.LicensePlate,
            new AnalyticsPeriodDto(startDate, endDate, daysInPeriod),
            new ReservationAnalyticsDto(
                totalReservations,
                completedReservations,
                cancelledReservations,
                activeReservations,
                averageReservationDuration,
                cancellationRate
            ),
            new RevenueAnalyticsDto(
                totalRevenue.Amount,
                averageRevenuePerReservation,
                daysInPeriod > 0 ? totalRevenue.Amount / daysInPeriod : 0,
                totalRevenue.Currency
            ),
            new UtilizationAnalyticsDto(
                daysReserved,
                daysInPeriod - daysReserved,
                0, // Days in maintenance - would need status history
                utilizationRate,
                100 - utilizationRate
            )
        );
    }

    public async Task<IEnumerable<Car>> GetCarsByLocationAsync(
        string city,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Reservations)
            .Where(c => c.CurrentLocation.City.ToLower().Contains(city.ToLower()))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Car>> GetCarsByStatusAsync(
        CarStatus status,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Reservations)
            .Where(c => c.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetTotalCarsCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cars.CountAsync(cancellationToken);
    }

    public async Task<int> GetAvailableCarsCountAsync(
        DateRange? period = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Cars
            .Include(c => c.Reservations)
            .Where(c => c.Status == CarStatus.Available);

        if (period != null)
        {
            var cars = await query.ToListAsync(cancellationToken);
            return cars.Count(car => !car.Reservations.Any(r => 
                r.Status == ReservationStatus.Active && 
                r.Period.OverlapsWith(period)));
        }

        return await query.CountAsync(cancellationToken);
    }
}
