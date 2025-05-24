namespace CarReservation.Application.DTOs;

public record CarDto(
    Guid Id,
    string Brand,
    string Model,
    string LicensePlate,
    string CarType,
    decimal PricePerDay,
    string Currency,
    string Status,
    LocationDto CurrentLocation,
    List<ReservationDto> Reservations
);
