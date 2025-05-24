namespace CarReservation.Application.Queries.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;

public record GetAllCarsQuery : IQuery<IEnumerable<CarSummaryDto>>;

public class GetAllCarsQueryHandler : IQueryHandler<GetAllCarsQuery, IEnumerable<CarSummaryDto>>
{
    private readonly ICarRepository _carRepository;

    public GetAllCarsQueryHandler(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<Result<IEnumerable<CarSummaryDto>>> Handle(
        GetAllCarsQuery request, 
        CancellationToken cancellationToken)
    {
        var cars = await _carRepository.GetAllAsync(cancellationToken);
        var carDtos = cars.ToSummaryDto();
        return Result<IEnumerable<CarSummaryDto>>.Success(carDtos);
    }
}
