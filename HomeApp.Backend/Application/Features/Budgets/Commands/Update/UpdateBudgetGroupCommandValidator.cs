using Domain.Entities.Budgets.Enums;
using FluentValidation;

namespace Application.Features.Budgets.Commands.Update;

internal sealed class UpdateBudgetGroupCommandValidator : AbstractValidator<UpdateBudgetGroupCommand>
{
    public UpdateBudgetGroupCommandValidator()
    {
        RuleFor(c => c.BudgetGroupId).GreaterThan(0);
        RuleFor(c => c.Index).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Title).NotEmpty()
            .WithMessage("Title is required.");
        // Unknown groups would be excluded from the E+A totals - force an explicit choice
        RuleFor(c => c.BudgetGroupType)
            .IsInEnum()
            .NotEqual(BudgetGroupType.Unknown)
            .WithMessage("BudgetGroupType must be Income or Expense.");
        RuleFor(c => c.TargetPercent)
            .InclusiveBetween(0, 100)
            .When(c => c.TargetPercent.HasValue);
    }
}
