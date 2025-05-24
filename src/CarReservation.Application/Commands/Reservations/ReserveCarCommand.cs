namespace CarReservation.Application.Commands.Reservations;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;
using CarReservation.Application.Extensions;

public record ReserveCarCommand(
    Guid CarId,
    Guid CustomerId,
    DateTime StartDate,
    DateTime EndDate
) : ICommand<ReservationDto>;

public class ReserveCarCommandHandler : ICommandHandler<ReserveCarCommand, ReservationDto>
{
    private readonly ICarRepository _carRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReserveCarCommandHandler(
        ICarRepository carRepository,
        IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReservationDto>> Handle(
        ReserveCarCommand request, 
        CancellationToken cancellationToken)
    {
        var carId = CarId.From(request.CarId);
        var car = await _carRepository.GetByIdAsync(carId, cancellationToken);
        
        if (car is null)
            return Result<ReservationDto>.Failure("Car not found");

        var customerId = CustomerId.From(request.CustomerId);
        var period = new DateRange(request.StartDate, request.EndDate);

        var reserveResult = car.Reserve(customerId, period);
        
        if (reserveResult.IsFailure)
            return Result<ReservationDto>.Failure(reserveResult.Error);

        await _carRepository.UpdateAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var reservation = car.Reservations.OrderByDescending(r => r.CreatedAt).First();
        var reservationDto = reservation.ToDto();
        
        return Result<ReservationDto>.Success(reservationDto);
    }
}
