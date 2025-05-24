namespace CarReservation.Application.Commands.Reservations;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record CreateReservationCommand(CreateReservationDto Reservation) : ICommand<ReservationDto>;

public class CreateReservationCommandHandler : ICommandHandler<CreateReservationCommand, ReservationDto>
{
    private readonly ICarRepository _carRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReservationCommandHandler(
        ICarRepository carRepository,
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReservationDto>> Handle(
        CreateReservationCommand request,
        CancellationToken cancellationToken)
    {
        var carId = CarId.From(request.Reservation.CarId);
        var customerId = CustomerId.From(request.Reservation.CustomerId);
        var period = request.Reservation.Period.ToEntity();

        var car = await _carRepository.GetByIdAsync(carId, cancellationToken);
        if (car is null)
        {
            return Result<ReservationDto>.Failure("Car not found");
        }

        var reservationResult = car.Reserve(customerId, period);
        if (reservationResult.IsFailure)
        {
            return Result<ReservationDto>.Failure(reservationResult.Error);
        }

        await _carRepository.UpdateAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var reservation = car.Reservations.Last();
        var reservationDto = reservation.ToDto();
        
        return Result<ReservationDto>.Success(reservationDto);
    }
}
