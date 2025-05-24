namespace CarReservation.Application.Validators;

using FluentValidation;
using CarReservation.Application.Commands.Cars;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(x => x.Car.Brand)
            .NotEmpty().WithMessage("Brand is required")
            .MaximumLength(100).WithMessage("Brand must not exceed 100 characters");

        RuleFor(x => x.Car.Model)
            .NotEmpty().WithMessage("Model is required")
            .MaximumLength(100).WithMessage("Model must not exceed 100 characters");

        RuleFor(x => x.Car.LicensePlate)
            .NotEmpty().WithMessage("License plate is required")
            .MaximumLength(20).WithMessage("License plate must not exceed 20 characters");

        RuleFor(x => x.Car.PricePerDay)
            .GreaterThan(0).WithMessage("Price per day must be greater than 0");

        RuleFor(x => x.Car.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be 3 characters");
    }
}
