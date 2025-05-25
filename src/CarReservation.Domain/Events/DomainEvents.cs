namespace CarReservation.Domain.Events;

using CarReservation.Domain.Common;
using CarReservation.Domain.Enums;
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

public record UserRegisteredDomainEvent(
    UserId UserId,
    string Email,
    UserRole Role
) : IDomainEvent;

public record UserProfileUpdatedDomainEvent(
    UserId UserId,
    UserProfile Profile
) : IDomainEvent;

public record UserLoggedInDomainEvent(
    UserId UserId,
    DateTime LoginTime
) : IDomainEvent;

public record UserPasswordChangedDomainEvent(
    UserId UserId
) : IDomainEvent;
