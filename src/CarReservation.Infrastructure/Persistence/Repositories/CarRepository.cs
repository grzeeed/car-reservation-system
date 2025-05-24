namespace CarReservation.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using CarReservation.Domain.Entities;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public class CarRepository : ICarRepository
{
    private readonly CarReservationDbContext _context;

    public CarRepository(CarReservationDbContext context)
    {
        _context = context;
    }

    public async Task<Car?> GetByIdAsync(CarId id, CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Reservations)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Cars
            .Include(c => c.Reservations)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Car car, CancellationToken cancellationToken = default)
    {
        await _context.Cars.AddAsync(car, cancellationToken);
    }

    public Task UpdateAsync(Car car, CancellationToken cancellationToken = default)
    {
        _context.Cars.Update(car);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Car car, CancellationToken cancellationToken = default)
    {
        _context.Cars.Remove(car);
        return Task.CompletedTask;
    }
}