namespace CarReservation.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using CarReservation.Domain.Entities;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public class ReservationRepository : IReservationRepository
{
    private readonly CarReservationDbContext _context;

    public ReservationRepository(CarReservationDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation?> GetByIdAsync(ReservationId id, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<List<Reservation>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _context.Reservations.AddAsync(reservation, cancellationToken);
    }

    public Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        _context.Reservations.Update(reservation);
        return Task.CompletedTask;
    }
}