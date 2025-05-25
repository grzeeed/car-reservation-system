namespace CarReservation.Infrastructure.Services;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CarReservation.Application.DTOs;
using CarReservation.Application.Interfaces;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _tokenLifetime;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _secretKey = _configuration["Jwt:SecretKey"] ?? "CarReservationSecretKeyForDevelopment123!";
        _issuer = _configuration["Jwt:Issuer"] ?? "CarReservationAPI";
        _audience = _configuration["Jwt:Audience"] ?? "CarReservationClient";
        _tokenLifetime = TimeSpan.FromHours(24); // Default 24 hours
    }

    public string GenerateToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role),
            new("IsActive", user.IsActive.ToString())
        };

        // Add profile claims if available
        if (user.Profile != null)
        {
            claims.Add(new Claim(ClaimTypes.GivenName, user.Profile.FirstName));
            claims.Add(new Claim(ClaimTypes.Surname, user.Profile.LastName));
            
            if (!string.IsNullOrEmpty(user.Profile.PhoneNumber))
                claims.Add(new Claim(ClaimTypes.MobilePhone, user.Profile.PhoneNumber));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_tokenLifetime),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public UserDto? GetUserFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);

            var userId = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var role = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var isActive = jsonToken.Claims.FirstOrDefault(x => x.Type == "IsActive")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
                return null;

            // Get profile information if available
            var firstName = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var lastName = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
            var phoneNumber = jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value;

            UserProfileDto? profile = null;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                profile = new UserProfileDto(firstName, lastName, phoneNumber, null);
            }

            return new UserDto(
                Id: Guid.Parse(userId),
                Email: email,
                Role: role,
                IsActive: bool.TryParse(isActive, out var active) && active,
                CreatedAt: DateTime.MinValue, // Not stored in token
                LastLoginAt: null, // Not stored in token
                Profile: profile
            );
        }
        catch
        {
            return null;
        }
    }
}
