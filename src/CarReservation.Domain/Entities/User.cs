namespace CarReservation.Domain.Entities;

using CarReservation.Domain.Common;
using CarReservation.Domain.Enums;
using CarReservation.Domain.ValueObjects;

public class User : Entity
{
    public UserId Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserRole Role { get; private set; }
    public string? Phone { get; private set; }
    public string? Department { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsProfileComplete { get; private set; }