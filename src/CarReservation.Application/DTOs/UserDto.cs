namespace CarReservation.Application.DTOs;

public record UserDto(
    Guid Id,
    string Email,
    string Role,
    UserProfileDto Profile,
    DateTime CreatedAt,
    DateTime? LastLoginAt,
    bool IsActive
);

public record UserProfileDto(
    string FirstName,
    string LastName,
    string? Phone,
    bool IsProfileComplete
);

public record LoginResponseDto(
    string Token,
    UserDto User
);

public record RegisterUserDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Phone,
    string? Department
);
