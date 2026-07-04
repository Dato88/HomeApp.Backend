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
    }
}
