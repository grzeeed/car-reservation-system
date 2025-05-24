namespace CarReservation.Application.Queries.Cars;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Interfaces;
using CarReservation.Domain.Common;

public record GetCarAnalyticsQuery(
    Guid CarId,
    DateTime StartDate,
    DateTime EndDate
) : IQuery<CarAnalyticsDto>;

public class GetCarAnalyticsQueryHandler : IQueryHandler<GetCarAnalyticsQuery, CarAnalyticsDto>
{
    private readonly ICarReadRepository _carReadRepository;

    public GetCarAnalyticsQueryHandler(ICarReadRepository carReadRepository)
    {
        _carReadRepository = carReadRepository;
    }

    public async Task<Result<CarAnalyticsDto>> Handle(GetCarAnalyticsQuery request, CancellationToken cancellationToken)
    {
        var analytics = await _carReadRepository.GetCarAnalyticsAsync(
            request.CarId, 
            request.StartDate, 
            request.EndDate, 
            cancellationToken);

        if (analytics == null)
            return Result<CarAnalyticsDto>.Failure("Car not found or no data available");

        return Result<CarAnalyticsDto>.Success(analytics);
    }
}
