namespace CarReservation.Application.Commands.Auth;

using CarReservation.Application.Common.Interfaces;
using CarReservation.Application.DTOs;
using CarReservation.Application.Interfaces;
using CarReservation.Domain.Common;
using CarReservation.Domain.Interfaces;
using CarReservation.Domain.ValueObjects;

public record ChangePasswordCommand(
    Guid UserId, 
    ChangePasswordDto PasswordData
) : ICommand;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ChangePasswordCommand request, 
        CancellationToken cancellationToken)
    {
        var userId = UserId.From(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        
        if (user == null)
            return Result.Failure("User not found");

        var passwordData = request.PasswordData;

        // Validate new passwords match
        if (passwordData.NewPassword != passwordData.ConfirmNewPassword)
            return Result.Failure("New passwords do not match");

        // Verify current password
        var isCurrentPasswordValid = _passwordHasher.VerifyPassword(
            passwordData.CurrentPassword, 
            user.PasswordHash);

        if (!isCurrentPasswordValid)
            return Result.Failure("Current password is incorrect");

        // Hash new password and update
        var newPasswordHash = _passwordHasher.HashPassword(passwordData.NewPassword);
        var result = user.ChangePassword(newPasswordHash);

        if (result.IsFailure)
            return result;

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
