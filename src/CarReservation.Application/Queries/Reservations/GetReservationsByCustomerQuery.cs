namespace CarReservation.Application.Queries.Reservations;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record GetReservationsByCustomerQuery(Guid CustomerId) : IQuery<IEnumerable<ReservationDto>>;

public class GetReservationsByCustomerQueryHandler : IQueryHandler<GetReservationsByCustomerQuery, IEnumerable<ReservationDto>>
{
    private readonly IReservationRepository _reservationRepository;

    public GetReservationsByCustomerQueryHandler(
        IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<Result<IEnumerable<ReservationDto>>> Handle(
        GetReservationsByCustomerQuery request, 
        CancellationToken cancellationToken)
    {
        var customerId = CustomerId.From(request.CustomerId);
        var reservations = await _reservationRepository.GetByCustomerIdAsync(customerId, cancellationToken);

        var reservationDtos = reservations.Select(x => x.ToDto()).ToArray();
        return Result<IEnumerable<ReservationDto>>.Success(reservationDtos);
    }
}
