namespace CarReservation.Application.Commands.Reservations;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record CancelReservationCommand(Guid ReservationId, string Reason) : ICommand;

public class CancelReservationCommandHandler : ICommandHandler<CancelReservationCommand>
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelReservationCommandHandler(
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
    {
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CancelReservationCommand request, 
        CancellationToken cancellationToken)
    {
        var reservationId = ReservationId.From(request.ReservationId);
        var reservation = await _reservationRepository.GetByIdAsync(reservationId, cancellationToken);
        
        if (reservation is null)
            return Result.Failure("Reservation not found");

        var cancelResult = reservation.Cancel(request.Reason);
        
        if (cancelResult.IsFailure)
            return cancelResult;

        await _reservationRepository.UpdateAsync(reservation, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
