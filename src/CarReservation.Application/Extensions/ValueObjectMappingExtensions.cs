using CarReservation.Application.DTOs;
using CarReservation.Domain.ValueObjects;

namespace CarReservation.Application.Extensions;

public static class ValueObjectMappingExtensions
{
    public static LocationDto ToDto(this Location location)
    {
        return new LocationDto(
            City: location.City,
            Address: location.Address,
            Latitude: location.Latitude,
            Longitude: location.Longitude
        );
    }

    public static Location ToEntity(this LocationDto dto)
    {
        return new Location(
            city: dto.City,
            address: dto.Address,
            latitude: dto.Latitude,
            longitude: dto.Longitude
        );
    }

    public static Location ToEntity(this CreateLocationDto dto)
    {
        return new Location(
            city: dto.City,
            address: dto.Address,
            latitude: dto.Latitude,
            longitude: dto.Longitude
        );
    }

    public static DateRangeDto ToDto(this DateRange dateRange)
    {
        return new DateRangeDto(
            StartDate: dateRange.StartDate,
            EndDate: dateRange.EndDate
        );
    }

    public static DateRange ToEntity(this DateRangeDto dto)
    {
        return new DateRange(
            startDate: dto.StartDate,
            endDate: dto.EndDate
        );
    }
}
