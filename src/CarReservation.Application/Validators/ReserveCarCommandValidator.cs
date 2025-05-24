namespace CarReservation.Application.Validators;

using FluentValidation;
using CarReservation.Application.Commands.Reservations;

public class ReserveCarCommandValidator : AbstractValidator<ReserveCarCommand>
{
    public ReserveCarCommandValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty().WithMessage("Car ID is required");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be after or equal to start date");
    }
}
