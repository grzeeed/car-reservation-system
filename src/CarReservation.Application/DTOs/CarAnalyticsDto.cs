namespace CarReservation.Application.DTOs;

public record CarAnalyticsDto(
    Guid CarId,
    string Brand,
    string Model,
    string LicensePlate,
    AnalyticsPeriodDto Period,
    ReservationAnalyticsDto ReservationStats,
    RevenueAnalyticsDto RevenueStats,
    UtilizationAnalyticsDto UtilizationStats
);

public record AnalyticsPeriodDto(
    DateTime StartDate,
    DateTime EndDate,
    int TotalDays
);

public record ReservationAnalyticsDto(
    int TotalReservations,
    int CompletedReservations,
    int CancelledReservations,
    int ActiveReservations,
    double AverageReservationDuration,
    double CancellationRate
);

public record RevenueAnalyticsDto(
    decimal TotalRevenue,
    decimal AverageRevenuePerReservation,
    decimal AverageRevenuePerDay,
    string Currency
);

public record UtilizationAnalyticsDto(
    int DaysReserved,
    int DaysAvailable,
    int DaysInMaintenance,
    double UtilizationRate,
    double AvailabilityRate
);
