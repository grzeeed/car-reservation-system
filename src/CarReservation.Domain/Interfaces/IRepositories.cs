namespace CarReservation.Domain.Interfaces;

using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;

public interface ICarRepository
{
    Task<Car?> GetByIdAsync(CarId id, CancellationToken cancellationToken = default);
    Task<List<Car>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Car car, CancellationToken cancellationToken = default);
    Task UpdateAsync(Car car, CancellationToken cancellationToken = default);
    Task DeleteAsync(Car car, CancellationToken cancellationToken = default);
}

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(ReservationId id, CancellationToken cancellationToken = default);
    Task<List<Reservation>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);
    Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);
}

public interface IUserRepository
{
    Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
