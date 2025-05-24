namespace CarReservation.Application.DTOs;

public record LocationDto(
    string City,
    string Address,
    double Latitude,
    double Longitude
);

public record CreateLocationDto(
    string City,
    string Address,
    double Latitude,
    double Longitude
);
