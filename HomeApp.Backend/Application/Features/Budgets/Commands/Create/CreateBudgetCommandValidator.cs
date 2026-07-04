using FluentValidation;

namespace Application.Features.Budgets.Commands.Create;

public class CreateBudgetCommandValidator : AbstractValidator<CreateBudgetCommand>
{
    public CreateBudgetCommandValidator()
    {
        RuleFor(c => c.HouseholdId).GreaterThan(0);
        RuleFor(c => c.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 10)
            .WithMessage("Year must be between 1900 and " + (DateTime.UtcNow.Year + 10) + ".");
    }
}
