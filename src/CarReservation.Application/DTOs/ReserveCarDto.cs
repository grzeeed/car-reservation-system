namespace CarReservation.Application.DTOs;

public record ReserveCarDto(
    Guid CarId,
    Guid CustomerId,
    DateTime StartDate,
    DateTime EndDate
);
