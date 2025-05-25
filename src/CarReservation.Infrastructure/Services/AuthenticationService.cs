namespace CarReservation.Infrastructure.Services;

using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Application.Interfaces;
using CarReservation.Domain.Interfaces;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthenticationService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
    }

    public async Task<AuthenticationResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email, cancellationToken);
            if (user == null)
                return new AuthenticationResultDto(false, null, null, "Invalid email or password");

            if (!user.IsActive)
                return new AuthenticationResultDto(false, null, null, "Account is deactivated");

            if (!_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
                return new AuthenticationResultDto(false, null, null, "Invalid email or password");

            // Record login
            var loginResult = user.RecordLogin();
            if (loginResult.IsFailure)
                return new AuthenticationResultDto(false, null, null, loginResult.Error);

            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userDto = user.ToDto();
            var token = _tokenService.GenerateToken(userDto);

            return new AuthenticationResultDto(true, token, userDto, null);
        }
        catch (Exception ex)
        {
            return new AuthenticationResultDto(false, null, null, "An error occurred during login");
        }
    }

    public async Task<AuthenticationResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Check if user already exists
            if (await _userRepository.ExistsAsync(registerDto.Email, cancellationToken))
                return new AuthenticationResultDto(false, null, null, "User with this email already exists");

            // Validate passwords match
            if (registerDto.Password != registerDto.ConfirmPassword)
                return new AuthenticationResultDto(false, null, null, "Passwords do not match");

            // Hash password
            var passwordHash = _passwordHasher.HashPassword(registerDto.Password);

            // Create user
            var userId = Domain.ValueObjects.UserId.Create();
            var user = new Domain.Entities.User(userId, registerDto.Email, passwordHash, Domain.Enums.UserRole.External);

            // Add profile if provided
            if (!string.IsNullOrWhiteSpace(registerDto.FirstName) && 
                !string.IsNullOrWhiteSpace(registerDto.LastName))
            {
                var profile = new Domain.ValueObjects.UserProfile(registerDto.FirstName, registerDto.LastName);
                var updateResult = user.UpdateProfile(profile);
                if (updateResult.IsFailure)
                    return new AuthenticationResultDto(false, null, null, updateResult.Error);
            }

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userDto = user.ToDto();
            var token = _tokenService.GenerateToken(userDto);

            return new AuthenticationResultDto(true, token, userDto, null);
        }
        catch (Exception ex)
        {
            return new AuthenticationResultDto(false, null, null, "An error occurred during registration");
        }
    }

    public async Task<bool> ValidateTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return _tokenService.ValidateToken(token);
    }

    public async Task<UserDto?> GetUserFromTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return _tokenService.GetUserFromToken(token);
    }
}
