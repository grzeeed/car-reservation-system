namespace CarReservation.Application.Interfaces;

using CarReservation.Domain.Entities;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
