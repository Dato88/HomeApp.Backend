using FluentValidation;

namespace Application.Features.Budgets.Commands.Update;

internal sealed class UpdateBudgetCellCommandValidator : AbstractValidator<UpdateBudgetCellCommand>
{
    public UpdateBudgetCellCommandValidator()
    {
        RuleFor(c => c.BudgetCellId).GreaterThan(0);
    }
}
