namespace CarReservation.Application.Commands.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record DeleteCarCommand(Guid CarId) : ICommand;

public class DeleteCarCommandHandler : ICommandHandler<DeleteCarCommand>
{
    private readonly ICarRepository _carRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCarCommandHandler(ICarRepository carRepository, IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCarCommand request, CancellationToken cancellationToken)
    {
        var carId = CarId.From(request.CarId);
        var car = await _carRepository.GetByIdAsync(carId, cancellationToken);

        if (car == null)
            return Result.Failure("Car not found");

        // Check if car has active reservations
        if (car.Reservations.Any(r => r.Status == Domain.Enums.ReservationStatus.Active || 
                                     r.Status == Domain.Enums.ReservationStatus.Pending))
        {
            return Result.Failure("Cannot delete car with active or pending reservations");
        }

        await _carRepository.DeleteAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
