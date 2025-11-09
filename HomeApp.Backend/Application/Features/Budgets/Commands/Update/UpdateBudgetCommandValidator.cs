using FluentValidation;

namespace Application.Features.Budgets.Commands.Update;

public class UpdateBudgetCommandValidator : AbstractValidator<UpdateBudgetCommand>
{
    public UpdateBudgetCommandValidator()
    {
        RuleFor(c => c.BudgetId).GreaterThan(0);
        RuleFor(c => c.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 10)
            .WithMessage("Year must be between 1900 and " + (DateTime.UtcNow.Year + 10) + ".");
    }
}
