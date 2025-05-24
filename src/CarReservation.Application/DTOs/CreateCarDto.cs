namespace CarReservation.Application.DTOs;

public record CreateCarDto(
    string Brand,
    string Model,
    string LicensePlate,
    string CarType,
    decimal PricePerDay,
    string Currency,
    CreateLocationDto Location
);

public record UpdateCarDto(
    Guid Id,
    string Brand,
    string Model,
    string LicensePlate,
    string CarType,
    decimal PricePerDay,
    string Currency,
    CreateLocationDto Location
);
