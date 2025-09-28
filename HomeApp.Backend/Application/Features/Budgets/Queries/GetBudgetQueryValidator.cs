using FluentValidation;

namespace Application.Features.Budgets.Queries;

public class GetBudgetQueryValidator : AbstractValidator<GetBudgetQuery>
{
    public GetBudgetQueryValidator()
    {
        RuleFor(c => c.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 10)
            .WithMessage("Year must be between 1900 and " + (DateTime.UtcNow.Year + 10) + ".");
    }
}
