namespace CarReservation.Domain.Entities;

using CarReservation.Domain.Common;
using CarReservation.Domain.Enums;
using CarReservation.Domain.Events;
using CarReservation.Domain.ValueObjects;

public class Car : AggregateRoot
{
    public CarId Id { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public string LicensePlate { get; private set; }
    public CarType Type { get; private set; }
    public Money PricePerDay { get; private set; }
    public CarStatus Status { get; private set; }
    public Location CurrentLocation { get; private set; }
    
    private readonly List<Reservation> _reservations = new();
    public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

    private Car() { } // For EF

    public Car(CarId id, string brand, string model, string licensePlate, 
               CarType type, Money pricePerDay, Location location)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        LicensePlate = licensePlate ?? throw new ArgumentNullException(nameof(licensePlate));
        Type = type;
        PricePerDay = pricePerDay ?? throw new ArgumentNullException(nameof(pricePerDay));
        CurrentLocation = location ?? throw new ArgumentNullException(nameof(location));
        Status = CarStatus.Available;

        AddDomainEvent(new CarCreatedDomainEvent(Id, Brand, Model, LicensePlate));
    }

    public Result<ReservationId> Reserve(CustomerId customerId, DateRange period)
    {
        if (Status != CarStatus.Available)
            return Result<ReservationId>.Failure("Car is not available for reservation");

        if (IsReservedForPeriod(period))
            return Result<ReservationId>.Failure("Car is already reserved for this period");

        var reservation = new Reservation(
            ReservationId.Create(),
            Id,
            customerId,
            period,
            CalculateTotalPrice(period)
        );

        _reservations.Add(reservation);
        Status = CarStatus.Reserved;
        
        AddDomainEvent(new CarReservedDomainEvent(Id, reservation.Id, customerId, period));

        return Result<ReservationId>.Success(reservation.Id);
    }

    public Result CancelReservation(ReservationId reservationId, string reason)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
            return Result.Failure("Reservation not found");

        var result = reservation.Cancel(reason);
        if (result.IsSuccess)
        {
            // If no other active reservations, make car available
            if (!_reservations.Any(r => r.Status == ReservationStatus.Active && r.Id != reservationId))
            {
                Status = CarStatus.Available;
            }
        }

        return result;
    }

    public Result SetMaintenance()
    {
        if (HasActiveReservations())
            return Result.Failure("Cannot set car to maintenance while it has active reservations");

        Status = CarStatus.InMaintenance;
        return Result.Success();
    }

    public Result SetAvailable()
    {
        if (Status == CarStatus.Reserved && HasActiveReservations())
            return Result.Failure("Car has active reservations and cannot be set to available");

        Status = CarStatus.Available;
        return Result.Success();
    }

    public Result SetOutOfService()
    {
        if (HasActiveReservations())
            return Result.Failure("Cannot set car out of service while it has active reservations");

        Status = CarStatus.OutOfService;
        return Result.Success();
    }

    public Result UpdateLocation(Location newLocation)
    {
        if (newLocation == null)
            return Result.Failure("Location cannot be null");

        CurrentLocation = newLocation;
        return Result.Success();
    }

    public Result UpdatePricing(Money newPricePerDay)
    {
        if (newPricePerDay == null)
            return Result.Failure("Price cannot be null");

        if (newPricePerDay.Amount <= 0)
            return Result.Failure("Price must be greater than zero");

        PricePerDay = newPricePerDay;
        return Result.Success();
    }

    public bool IsAvailableForPeriod(DateRange period)
    {
        return Status == CarStatus.Available && !IsReservedForPeriod(period);
    }

    public int GetActiveReservationsCount()
    {
        return _reservations.Count(r => r.Status == ReservationStatus.Active);
    }

    public Money CalculateRevenueForPeriod(DateRange period)
    {
        var relevantReservations = _reservations
            .Where(r => r.Status == ReservationStatus.Completed && 
                       r.Period.OverlapsWith(period))
            .ToList();

        if (!relevantReservations.Any())
            return new Money(0, PricePerDay.Currency);

        var totalAmount = relevantReservations.Sum(r => r.TotalPrice.Amount);
        return new Money(totalAmount, PricePerDay.Currency);
    }

    private bool IsReservedForPeriod(DateRange period)
    {
        return _reservations.Any(r => 
            r.Status == ReservationStatus.Active && 
            r.Period.OverlapsWith(period));
    }

    private bool HasActiveReservations()
    {
        return _reservations.Any(r => r.Status == ReservationStatus.Active
            || r.Status == ReservationStatus.Pending
            || r.Status == ReservationStatus.Completed);
    }

    private Money CalculateTotalPrice(DateRange period)
    {
        var days = period.Days;
        return PricePerDay * days;
    }
}
