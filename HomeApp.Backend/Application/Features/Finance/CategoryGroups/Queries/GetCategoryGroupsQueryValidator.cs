using FluentValidation;

namespace Application.Features.Finance.CategoryGroups.Queries;

internal sealed class GetCategoryGroupsQueryValidator : AbstractValidator<GetCategoryGroupsQuery>
{
    public GetCategoryGroupsQueryValidator()
    {
        RuleFor(c => c.HouseholdId).GreaterThan(0);
    }
}
