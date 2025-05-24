namespace CarReservation.Domain.Events;

using CarReservation.Domain.Common;
using CarReservation.Domain.ValueObjects;

public record CarCreatedDomainEvent(
    CarId CarId,
    string Brand,
    string Model,
    string LicensePlate
) : IDomainEvent;

public record CarReservedDomainEvent(
    CarId CarId,
    ReservationId ReservationId,
    CustomerId CustomerId,
    DateRange Period
) : IDomainEvent;

public record ReservationConfirmedDomainEvent(
    ReservationId ReservationId,
    CarId CarId,
    CustomerId CustomerId
) : IDomainEvent;

public record ReservationCancelledDomainEvent(
    ReservationId ReservationId,
    CarId CarId,
    CustomerId CustomerId,
    string Reason
) : IDomainEvent;
