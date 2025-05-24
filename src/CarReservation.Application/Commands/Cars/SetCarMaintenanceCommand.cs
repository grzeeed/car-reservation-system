namespace CarReservation.Application.Commands.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record SetCarMaintenanceCommand(Guid CarId) : ICommand;

public class SetCarMaintenanceCommandHandler : ICommandHandler<SetCarMaintenanceCommand>
{
    private readonly ICarRepository _carRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetCarMaintenanceCommandHandler(ICarRepository carRepository, IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SetCarMaintenanceCommand request, CancellationToken cancellationToken)
    {
        var carId = CarId.From(request.CarId);
        var car = await _carRepository.GetByIdAsync(carId, cancellationToken);

        if (car == null)
            return Result.Failure("Car not found");

        var result = car.SetMaintenance();
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _carRepository.UpdateAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
