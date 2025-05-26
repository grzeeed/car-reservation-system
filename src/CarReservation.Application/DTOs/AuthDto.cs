namespace CarReservation.Application.DTOs;

public record LoginDto(
    string Email,
    string Password
);

public record RegisterDto(
    string Email,
    string Password,
    string ConfirmPassword,
    string FirstName,
    string LastName,
    string? Phone = null,
    string? Department = null
);

public record AuthResponseDto(
    string Token,
    DateTime ExpiresAt,
    UserDto User
);

public record ChangePasswordDto(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
);

public record UpdateProfileDto(
    string FirstName,
    string LastName,
    string? Phone,
    string? Department
);
