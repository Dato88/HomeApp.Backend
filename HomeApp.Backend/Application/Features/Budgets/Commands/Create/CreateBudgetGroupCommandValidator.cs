using FluentValidation;

namespace Application.Features.Budgets.Commands.Create;

internal sealed class CreateBudgetGroupCommandValidator : AbstractValidator<CreateBudgetGroupCommand>
{
    public CreateBudgetGroupCommandValidator()
    {
        RuleFor(c => c.BudgetId).GreaterThan(0);
        RuleFor(c => c.Index).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Name).NotEmpty()
            .WithMessage("Name is required.");
        RuleFor(c => c.TargetPercent)
            .InclusiveBetween(0, 100)
            .When(c => c.TargetPercent.HasValue);
    }
}
