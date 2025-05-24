namespace CarReservation.Application.Commands.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;

public record CreateCarCommand(CreateCarDto Car) : ICommand<CarDto>;

public class CreateCarCommandHandler : ICommandHandler<CreateCarCommand, CarDto>
{
    private readonly ICarRepository _carRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCarCommandHandler(
        ICarRepository carRepository,
        IUnitOfWork unitOfWork)
    {
        _carRepository = carRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CarDto>> Handle(
        CreateCarCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var car = request.Car.ToEntity();
            
            await _carRepository.AddAsync(car, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var carDto = car.ToDto();
            return Result<CarDto>.Success(carDto);
        }
        catch (ArgumentException ex)
        {
            return Result<CarDto>.Failure(ex.Message);
        }
    }
}
