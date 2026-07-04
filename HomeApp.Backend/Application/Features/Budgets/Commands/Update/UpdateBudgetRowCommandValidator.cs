using FluentValidation;

namespace Application.Features.Budgets.Commands.Update;

internal sealed class UpdateBudgetRowCommandValidator : AbstractValidator<UpdateBudgetRowCommand>
{
    public UpdateBudgetRowCommandValidator()
    {
        RuleFor(c => c.BudgetRowId).GreaterThan(0);
        RuleFor(c => c.Index).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Title).NotEmpty()
            .WithMessage("Title is required.");
    }
}
