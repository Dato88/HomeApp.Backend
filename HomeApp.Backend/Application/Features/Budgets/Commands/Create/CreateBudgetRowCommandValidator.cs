using FluentValidation;

namespace Application.Features.Budgets.Commands.Create;

internal sealed class CreateBudgetRowCommandValidator : AbstractValidator<CreateBudgetRowCommand>
{
    public CreateBudgetRowCommandValidator()
    {
        RuleFor(c => c.BudgetGroupId).GreaterThan(0);
        RuleFor(c => c.Index).GreaterThan(0);
        RuleFor(c => c.Name).NotEmpty()
            .WithMessage("Name is required.");
    }
}
