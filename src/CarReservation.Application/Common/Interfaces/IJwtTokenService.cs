namespace CarReservation.Application.Common.Interfaces;

using CarReservation.Domain.Entities;

public interface IJwtTokenService
{
    string GenerateToken(User user);
    bool ValidateToken(string token);
    string? GetUserIdFromToken(string token);
}
