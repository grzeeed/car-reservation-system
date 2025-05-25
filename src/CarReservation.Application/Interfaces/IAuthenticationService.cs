namespace CarReservation.Application.Interfaces;

using CarReservation.Application.DTOs;

public interface IAuthenticationService
{
    Task<AuthenticationResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    Task<AuthenticationResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
    Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<UserDto?> GetUserFromTokenAsync(string token, CancellationToken cancellationToken = default);
}

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public interface ITokenService
{
    string GenerateToken(UserDto user);
    bool ValidateToken(string token);
    UserDto? GetUserFromToken(string token);
}
