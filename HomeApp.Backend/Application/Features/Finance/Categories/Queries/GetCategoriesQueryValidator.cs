using FluentValidation;

namespace Application.Features.Finance.Categories.Queries;

internal sealed class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator()
    {
        RuleFor(c => c.HouseholdId).GreaterThan(0);
    }
}
