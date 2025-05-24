using CarReservation.Application.DTOs;
using FluentValidation;

namespace CarReservation.Application.Validators;

public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
{
    public CreateReservationDtoValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty()
            .WithMessage("Car ID is required");

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .WithMessage("Customer ID is required");

        RuleFor(x => x.Period)
            .NotNull()
            .WithMessage("Period is required")
            .SetValidator(new DateRangeDtoValidator());
    }
}

public class DateRangeDtoValidator : AbstractValidator<DateRangeDto>
{
    public DateRangeDtoValidator()
    {
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required")
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be after or equal to start date");

        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).Days <= 365)
            .WithMessage("Reservation period cannot exceed 365 days");
    }
}
