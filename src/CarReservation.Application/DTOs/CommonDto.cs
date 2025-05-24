namespace CarReservation.Application.DTOs;

public record DateRangeDto(
    DateTime StartDate,
    DateTime EndDate
);

public record CustomerDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone
);
