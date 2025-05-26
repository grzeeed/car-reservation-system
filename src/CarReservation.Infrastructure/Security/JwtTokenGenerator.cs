namespace CarReservation.Infrastructure.Security;

using CarReservation.Application.Interfaces;
using CarReservation.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user) 
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? "YourDefaultSecretKeyForDevelopment123!");
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.Value.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(ClaimTypes.Name, user.Profile.FullName),
            new Claim("FirstName", user.Profile.FirstName),
            new Claim("LastName", user.Profile.LastName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = _configuration["Jwt:Issuer"] ?? "CarReservationAPI",
            Audience = _configuration["Jwt:Audience"] ?? "CarReservationClient"
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
