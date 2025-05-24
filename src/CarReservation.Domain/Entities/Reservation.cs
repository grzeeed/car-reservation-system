namespace CarReservation.Domain.Entities;

using CarReservation.Domain.Common;
using CarReservation.Domain.Enums;
using CarReservation.Domain.Events;
using CarReservation.Domain.ValueObjects;

public class Reservation : Entity
{
    public ReservationId Id { get; private set; }
    public CarId CarId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public DateRange Period { get; private set; }
    public Money TotalPrice { get; private set; }
    public ReservationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    private Reservation() { } // For EF

    public Reservation(ReservationId id, CarId carId, CustomerId customerId, 
                      DateRange period, Money totalPrice)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        CarId = carId ?? throw new ArgumentNullException(nameof(carId));
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        Period = period ?? throw new ArgumentNullException(nameof(period));
        TotalPrice = totalPrice ?? throw new ArgumentNullException(nameof(totalPrice));
        Status = ReservationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public Result Confirm()
    {
        if (Status != ReservationStatus.Pending)
            return Result.Failure("Reservation cannot be confirmed");

        Status = ReservationStatus.Active;
        ConfirmedAt = DateTime.UtcNow;

        AddDomainEvent(new ReservationConfirmedDomainEvent(Id, CarId, CustomerId));
        return Result.Success();
    }

    public Result Cancel(string reason)
    {
        if (Status == ReservationStatus.Cancelled)
            return Result.Failure("Reservation is already cancelled");

        if (Status == ReservationStatus.Completed)
            return Result.Failure("Completed reservation cannot be cancelled");

        Status = ReservationStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;

        AddDomainEvent(new ReservationCancelledDomainEvent(Id, CarId, CustomerId, reason));
        return Result.Success();
    }
}
