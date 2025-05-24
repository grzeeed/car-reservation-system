namespace CarReservation.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using CarReservation.Domain.Entities;
using CarReservation.Domain.Common;
using System.Reflection;

public class CarReservationDbContext : DbContext
{
    public DbSet<Car> Cars { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    public CarReservationDbContext(DbContextOptions<CarReservationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .SelectMany(e => e.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        // Clear domain events after saving
        foreach (var entity in ChangeTracker.Entries<Entity>().Select(e => e.Entity))
        {
            entity.ClearDomainEvents();
        }

        // TODO: Dispatch domain events here if using a domain event dispatcher

        return result;
    }
}