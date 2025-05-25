namespace CarReservation.Application.Commands.Auth;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Application.Services;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;

public record LoginCommand(LoginDto LoginDto) : ICommand<LoginResponseDto>;

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

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.LoginDto.Email, cancellationToken);
        
        if (user == null)
            return Result<LoginResponseDto>.Failure("Invalid email or password");

        if (!user.IsActive)
            return Result<LoginResponseDto>.Failure("User account is deactivated");

        if (!_passwordHasher.VerifyPassword(request.LoginDto.Password, user.PasswordHash))
            return Result<LoginResponseDto>.Failure("Invalid email or password");

        user.RecordLogin();
        await _userRepository.UpdateAsync(user, cancellationToken);

        var token = _jwtTokenGenerator.GenerateToken(user.Id.Value, user.Email, user.Role.ToString());
        var userDto = user.ToDto();

        var response = new LoginResponseDto(token, userDto);
        return Result<LoginResponseDto>.Success(response);
    }
}
