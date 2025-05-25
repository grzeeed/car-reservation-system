namespace CarReservation.Domain.Entities;

using CarReservation.Domain.Common;
using CarReservation.Domain.Enums;
using CarReservation.Domain.Events;
using CarReservation.Domain.ValueObjects;

public class User : AggregateRoot
{
    public UserId Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public UserProfile Profile { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public bool IsActive { get; private set; }

    private User() { } // For EF

    public User(UserId id, string email, string passwordHash, UserRole role, UserProfile profile)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Email = email?.Trim().ToLowerInvariant() ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        Role = role;
        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        CreatedAt = DateTime.UtcNow;
        IsActive = true;

        ValidateEmail(Email);
        
        AddDomainEvent(new UserRegisteredDomainEvent(Id, Email, Role));
    }

    public Result UpdateProfile(UserProfile newProfile)
    {
        if (newProfile == null)
            return Result.Failure("Profile cannot be null");

        Profile = newProfile;
        
        AddDomainEvent(new UserProfileUpdatedDomainEvent(Id, Profile));
        return Result.Success();
    }

    public Result ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            return Result.Failure("Password hash cannot be empty");

        PasswordHash = newPasswordHash;
        
        AddDomainEvent(new UserPasswordChangedDomainEvent(Id));
        return Result.Success();
    }

    public Result UpdateRole(UserRole newRole)
    {
        if (Role == newRole)
            return Result.Success();

        Role = newRole;
        return Result.Success();
    }

    public void RecordLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        AddDomainEvent(new UserLoggedInDomainEvent(Id, LastLoginAt.Value));
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Failure("User is already deactivated");

        IsActive = false;
        return Result.Success();
    }

    public Result Activate()
    {
        if (IsActive)
            return Result.Failure("User is already active");

        IsActive = true;
        return Result.Success();
    }

    public bool VerifyPassword(string passwordHash)
    {
        return PasswordHash == passwordHash;
    }

    public bool HasRole(UserRole role)
    {
        return Role == role;
    }

    public bool HasAnyRole(params UserRole[] roles)
    {
        return roles.Contains(Role);
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        if (!email.Contains("@") || !email.Contains("."))
            throw new ArgumentException("Email format is invalid");

        if (email.Length > 254)
            throw new ArgumentException("Email is too long");
    }
}
