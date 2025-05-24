namespace CarReservation.Application.Commands.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record SetCarAvailableCommand(Guid CarId) : ICommand;

public class SetCarAvailableCommandHandler : ICommandHandler<SetCarAvailableCommand>
{
    private readonly ICarRepository _carRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetCarAvailableCommandHandler(ICarRepository carRepository, IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SetCarAvailableCommand request, CancellationToken cancellationToken)
    {
        var carId = CarId.From(request.CarId);
        var car = await _carRepository.GetByIdAsync(carId, cancellationToken);

        if (car == null)
            return Result.Failure("Car not found");

        var result = car.SetAvailable();
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _carRepository.UpdateAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
