namespace CarReservation.Infrastructure.Persistence;

using CarReservation.Domain.Interfaces;

public class UnitOfWork : IUnitOfWork
{
    private readonly CarReservationDbContext _context;

    public UnitOfWork(CarReservationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}