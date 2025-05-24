namespace CarReservation.Application.Commands.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record UpdateCarLocationCommand(
    Guid CarId,
    string City,
    string Address,
    double Latitude,
    double Longitude
) : ICommand;

public class UpdateCarLocationCommandHandler : ICommandHandler<UpdateCarLocationCommand>
{
    private readonly ICarRepository _carRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCarLocationCommandHandler(ICarRepository carRepository, IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCarLocationCommand request, CancellationToken cancellationToken)
    {
        var carId = CarId.From(request.CarId);
        var car = await _carRepository.GetByIdAsync(carId, cancellationToken);

        if (car == null)
            return Result.Failure("Car not found");

        var newLocation = new Location(request.City, request.Address, request.Latitude, request.Longitude);
        var result = car.UpdateLocation(newLocation);
        
        if (result.IsFailure)
            return Result.Failure(result.Error);

        await _carRepository.UpdateAsync(car, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
