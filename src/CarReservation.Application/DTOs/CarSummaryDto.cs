using CarReservation.Domain.Entities;
using CarReservation.Domain.Enums;

namespace CarReservation.Application.DTOs;

public record CarSummaryDto(
Guid Id,
string Brand,
    string Model,
    string LicensePlate,
    string CarType,
    decimal PricePerDay,
    string Currency,
    string Status,
    LocationDto CurrentLocation
);
