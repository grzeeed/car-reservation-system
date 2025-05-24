namespace CarReservation.Application.Queries.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Application.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Enums;
using CarReservation.Domain.ValueObjects;

public record GetAvailableCarsQuery(
    DateTime StartDate,
    DateTime EndDate,
    CarType? Type = null,
    string? Location = null
) : IQuery<IEnumerable<CarDto>>;

public class GetAvailableCarsQueryHandler : IQueryHandler<GetAvailableCarsQuery, IEnumerable<CarDto>>
{
    private readonly ICarReadRepository _carReadRepository;

    public GetAvailableCarsQueryHandler(
        ICarReadRepository carReadRepository)
    {
        _carReadRepository = carReadRepository;
    }

    public async Task<Result<IEnumerable<CarDto>>> Handle(
        GetAvailableCarsQuery request, 
        CancellationToken cancellationToken)
    {
        var period = new DateRange(request.StartDate, request.EndDate);
        
        var cars = await _carReadRepository.GetAvailableCarsAsync(
            period, 
            request.Type, 
            request.Location, 
            cancellationToken);

        var carDtos = cars.Select(c => c.ToDto()).ToArray();
        return Result<IEnumerable<CarDto>>.Success(carDtos);
    }
}
