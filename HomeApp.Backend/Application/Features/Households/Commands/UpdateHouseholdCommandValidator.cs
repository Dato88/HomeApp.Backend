using FluentValidation;

namespace Application.Features.Households.Commands;

internal sealed class UpdateHouseholdCommandValidator : AbstractValidator<UpdateHouseholdCommand>
{
    public UpdateHouseholdCommandValidator()
    {
        RuleFor(c => c.HouseholdId).GreaterThan(0);
        RuleFor(c => c.Name).NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(150);
    }
}
