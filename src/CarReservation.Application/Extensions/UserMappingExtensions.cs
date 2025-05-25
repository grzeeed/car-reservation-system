using CarReservation.Application.DTOs;
using CarReservation.Domain.Entities;
using CarReservation.Domain.Enums;
using CarReservation.Domain.ValueObjects;

namespace CarReservation.Application.Extensions;

public static class UserMappingExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            Id: user.Id.Value,
            Email: user.Email,
            Role: user.Role.ToString(),
            IsActive: user.IsActive,
            CreatedAt: user.CreatedAt,
            LastLoginAt: user.LastLoginAt,
            Profile: user.Profile?.ToDto()
        );
    }

    public static UserProfileDto ToDto(this UserProfile profile)
    {
        return new UserProfileDto(
            FirstName: profile.FirstName,
            LastName: profile.LastName,
            PhoneNumber: profile.PhoneNumber,
            Address: profile.Address?.ToDto()
        );
    }

    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto(
            Street: address.Street,
            City: address.City,
            PostalCode: address.PostalCode,
            Country: address.Country
        );
    }

    public static UserProfile ToEntity(this UserProfileDto dto)
    {
        return new UserProfile(
            firstName: dto.FirstName,
            lastName: dto.LastName,
            phoneNumber: dto.PhoneNumber,
            address: dto.Address?.ToEntity()
        );
    }

    public static Address ToEntity(this AddressDto dto)
    {
        return new Address(
            street: dto.Street,
            city: dto.City,
            postalCode: dto.PostalCode,
            country: dto.Country
        );
    }

    public static UserProfile ToEntity(this UpdateUserProfileDto dto)
    {
        return new UserProfile(
            firstName: dto.FirstName,
            lastName: dto.LastName,
            phoneNumber: dto.PhoneNumber,
            address: dto.Address?.ToEntity()
        );
    }

    public static List<UserDto> ToDto(this IEnumerable<User> users)
    {
        return users.Select(user => user.ToDto()).ToList();
    }

    public static UserRole ParseUserRole(string role)
    {
        return Enum.TryParse<UserRole>(role, true, out var userRole) 
            ? userRole 
            : UserRole.External;
    }
}
