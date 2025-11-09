using FluentValidation;

namespace Application.Features.Budgets.Commands.Create;

internal sealed class CreateBudgetCellCommandValidator : AbstractValidator<CreateBudgetCellCommand>
{
    public CreateBudgetCellCommandValidator()
    {
        RuleFor(c => c.BudgetRowId).GreaterThan(0);
        RuleFor(c => c.Amount).NotEmpty();
        RuleFor(c => c.Month).InclusiveBetween(1, 12)
            .WithMessage("Month must be between 1 and 12.");
    }
}
