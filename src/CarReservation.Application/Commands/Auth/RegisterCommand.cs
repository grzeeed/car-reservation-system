namespace CarReservation.Application.Commands.Auth;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.Entities;
using CarReservation.Domain.ValueObjects;
using CarReservation.Domain.Enums;

public record RegisterCommand(RegisterDto RegisterData) : ICommand<AuthResponseDto>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthResponseDto>> Handle(
        RegisterCommand request, 
        CancellationToken cancellationToken)
    {
        var registerData = request.RegisterData;

        // Validate passwords match
        if (registerData.Password != registerData.ConfirmPassword)
            return Result<AuthResponseDto>.Failure("Passwords do not match");

        // Check if user already exists
        var existingUser = await _userRepository.GetByEmailAsync(registerData.Email, cancellationToken);
        if (existingUser != null)
            return Result<AuthResponseDto>.Failure("User with this email already exists");

        // Hash password
        var passwordHash = _passwordHasher.HashPassword(registerData.Password);

        // Create user profile
        var profile = new UserProfile(
            registerData.FirstName,
            registerData.LastName,
            registerData.Phone
        );

        // Create user
        var userId = UserId.Create();
        var user = new User(
            userId,
            registerData.Email,
            passwordHash,
            UserRole.External, // Default role for registration
            profile
        );

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Generate token
        var token = _jwtTokenGenerator.GenerateToken(user);
        var expiresAt = DateTime.UtcNow.AddHours(24); // 24 hours

        var userDto = new UserDto(
            user.Id.Value,
            user.Email,
            user.Role.ToString(),
            new UserProfileDto(
            user.Profile.FirstName,
            user.Profile.LastName,
            user.Profile.Phone,
            user.Profile.IsProfileComplete),
            user.CreatedAt,
            user.LastLoginAt,
            user.IsActive
        );

        var response = new AuthResponseDto(token, expiresAt, userDto);

        return Result<AuthResponseDto>.Success(response);
    }
}
