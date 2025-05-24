using CarReservation.Application.DTOs;
using CarReservation.Domain.Enums;
using FluentValidation;

namespace CarReservation.Application.Validators;

public class CreateCarDtoValidator : AbstractValidator<CreateCarDto>
{
    public CreateCarDtoValidator()
    {
        RuleFor(x => x.Brand)
            .NotEmpty()
            .WithMessage("Brand is required")
            .MaximumLength(50)
            .WithMessage("Brand cannot exceed 50 characters");

        RuleFor(x => x.Model)
            .NotEmpty()
            .WithMessage("Model is required")
            .MaximumLength(50)
            .WithMessage("Model cannot exceed 50 characters");

        RuleFor(x => x.LicensePlate)
            .NotEmpty()
            .WithMessage("License plate is required")
            .MaximumLength(15)
            .WithMessage("License plate cannot exceed 15 characters");

        RuleFor(x => x.CarType)
            .NotEmpty()
            .WithMessage("Car type is required")
            .Must(BeValidCarType)
            .WithMessage("Invalid car type. Valid types: " + 
                string.Join(", ", Enum.GetNames<CarType>()));

        RuleFor(x => x.PricePerDay)
            .GreaterThan(0)
            .WithMessage("Price per day must be greater than 0")
            .LessThan(10000)
            .WithMessage("Price per day cannot exceed 10,000");

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithMessage("Currency is required")
            .Length(3)
            .WithMessage("Currency must be 3 characters (e.g., USD, EUR)");

        RuleFor(x => x.Location)
            .NotNull()
            .WithMessage("Location is required")
            .SetValidator(new CreateLocationDtoValidator());
    }

    private static bool BeValidCarType(string carType)
    {
        return Enum.TryParse<CarType>(carType, out _);
    }
}

public class CreateLocationDtoValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationDtoValidator()
    {
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City cannot exceed 100 characters");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(200)
            .WithMessage("Address cannot exceed 200 characters");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90)
            .WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180)
            .WithMessage("Longitude must be between -180 and 180");
    }
}
