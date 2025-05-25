namespace CarReservation.Application.Commands.Auth;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Extensions;
using CarReservation.Application.Services;
using CarReservation.Domain.Common;
using CarReservation.Domain.Entities;
using CarReservation.Domain.Enums;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record RegisterCommand(RegisterDto RegisterDto) : ICommand<UserDto>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = request.RegisterDto;

        // Validate passwords match
        if (dto.Password != dto.ConfirmPassword)
            return Result<UserDto>.Failure("Passwords do not match");

        // Check if user exists
        if (await _userRepository.ExistsWithEmailAsync(dto.Email, cancellationToken))
            return Result<UserDto>.Failure("User with this email already exists");

        try
        {
            // Create user
            var userId = UserId.Create();
            var passwordHash = _passwordHasher.HashPassword(dto.Password);
            var profile = new UserProfile(dto.FirstName, dto.LastName);
            
            var user = new User(userId, dto.Email, passwordHash, UserRole.Employee, profile);

            await _userRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var userDto = user.ToDto();
            return Result<UserDto>.Success(userDto);
        }
        catch (ArgumentException ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
    }
}
