using CarReservation.Application.DTOs;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;

namespace CarReservation.Application.Extensions;

public static class ReservationMappingExtensions
{
    public static ReservationDto ToDto(this Reservation reservation)
    {
        return new ReservationDto(
            Id: reservation.Id.Value,
            CarId: reservation.CarId.Value,
            CustomerId: reservation.CustomerId.Value,
            Period: reservation.Period.ToDto(),
            TotalAmount: reservation.TotalPrice.Amount,
            Currency: reservation.TotalPrice.Currency,
            Status: reservation.Status.ToString(),
            CreatedAt: reservation.CreatedAt,
            ConfirmedAt: reservation.ConfirmedAt,
            CancelledAt: reservation.CancelledAt
        );
    }

    public static ReservationSummaryDto ToSummaryDto(this Reservation reservation)
    {
        return new ReservationSummaryDto(
            Id: reservation.Id.Value,
            CarId: reservation.CarId.Value,
            Period: reservation.Period.ToDto(),
            TotalAmount: reservation.TotalPrice.Amount,
            Currency: reservation.TotalPrice.Currency,
            Status: reservation.Status.ToString(),
            CreatedAt: reservation.CreatedAt
        );
    }

    public static List<ReservationDto> ToDto(this IEnumerable<Reservation> reservations)
    {
        return reservations.Select(r => r.ToDto()).ToList();
    }

    public static List<ReservationSummaryDto> ToSummaryDto(this IEnumerable<Reservation> reservations)
    {
        return reservations.Select(r => r.ToSummaryDto()).ToList();
    }
}
