namespace CarReservation.Application.DTOs;

public record ReservationDto(
    Guid Id,
    Guid CarId,
    Guid CustomerId,
    DateRangeDto Period,
    decimal TotalAmount,
    string Currency,
    string Status,
    DateTime CreatedAt,
    DateTime? ConfirmedAt,
    DateTime? CancelledAt
);

public record ReservationSummaryDto(
    Guid Id,
    Guid CarId,
    DateRangeDto Period,
    decimal TotalAmount,
    string Currency,
    string Status,
    DateTime CreatedAt
);

public record CreateReservationDto(
    Guid CarId,
    Guid CustomerId,
    DateRangeDto Period
);

public record ConfirmReservationDto(
    Guid ReservationId
);

public record CancelReservationDto(
    Guid ReservationId,
    string Reason
);
