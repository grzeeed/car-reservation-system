namespace CarReservation.Application.Commands.Auth;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponseDto>;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<LoginResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // Validate email format
        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
            return Result<LoginResponseDto>.Failure("Invalid email address");

        // Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email.ToLowerInvariant(), cancellationToken);
        
        if (user is null)
            return Result<LoginResponseDto>.Failure("Invalid credentials");

        // Verify password
        var passwordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        
        if (!passwordValid)
            return Result<LoginResponseDto>.Failure("Invalid credentials");

        // Check if user is active
        if (!user.IsActive)
            return Result<LoginResponseDto>.Failure("User account is deactivated");

        // Record login
        user.RecordLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Generate JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        // Create response
        var userDto = new UserDto(
            Id: user.Id.Value,
            Email: user.Email,
            Role: user.Role.ToString(),
            Profile: new UserProfileDto(
                FirstName: user.Profile.FirstName,
                LastName: user.Profile.LastName,
                Phone: user.Profile.Phone,
                IsProfileComplete: user.Profile.IsProfileComplete
            ),
            CreatedAt: user.CreatedAt,
            LastLoginAt: user.LastLoginAt,
            IsActive: user.IsActive
        );

        var response = new LoginResponseDto(
            Token: token,
            User: userDto
        );

        return Result<LoginResponseDto>.Success(response);
    }
}
