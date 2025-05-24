namespace CarReservation.Application.Queries.Reservations;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record GetReservationByIdQuery(Guid Id) : IQuery<ReservationDto>;

public class GetReservationByIdQueryHandler : IQueryHandler<GetReservationByIdQuery, ReservationDto>
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationByIdQueryHandler(
        IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<Result<ReservationDto>> Handle(
        GetReservationByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var reservationId = ReservationId.From(request.Id);
        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        
        if (reservation is null)
            return Result<ReservationDto>.Failure("Reservation not found");

        var reservationDto = reservation.ToDto();
        return Result<ReservationDto>.Success(reservationDto);
    }
}
