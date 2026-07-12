using FluentValidation;

namespace Application.Features.Finance.Reports.Queries;

internal sealed class GetEvaReportQueryValidator : AbstractValidator<GetEvaReportQuery>
{
    public GetEvaReportQueryValidator()
    {
        RuleFor(c => c.HouseholdIds)
            .NotEmpty()
            .WithMessage("At least one HouseholdId is required.")
            .Must(ids => ids.Count <= 10)
            .WithMessage("At most 10 HouseholdIds are allowed.");
        RuleForEach(c => c.HouseholdIds).GreaterThan(0);
        RuleFor(c => c.Year)
            .InclusiveBetween(1900, DateTime.UtcNow.Year + 10)
            .WithMessage("Year must be between 1900 and " + (DateTime.UtcNow.Year + 10) + ".");
    }
}
