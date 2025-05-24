namespace CarReservation.Application.Queries.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record GetCarByIdQuery(Guid CarId) : IQuery<CarDto?>;

public class GetCarByIdQueryHandler : IQueryHandler<GetCarByIdQuery, CarDto?>
{
    private readonly ICarRepository _carRepository;

    public GetCarByIdQueryHandler(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<Result<CarDto?>> Handle(
        GetCarByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var carId = CarId.From(request.CarId);
        var car = await _carRepository.GetByIdAsync(carId, cancellationToken);
        
        var carDto = car?.ToDto();
        return Result<CarDto?>.Success(carDto);
    }
}
