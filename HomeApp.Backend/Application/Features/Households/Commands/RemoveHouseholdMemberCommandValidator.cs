using FluentValidation;

namespace Application.Features.Households.Commands;

internal sealed class RemoveHouseholdMemberCommandValidator : AbstractValidator<RemoveHouseholdMemberCommand>
{
    public RemoveHouseholdMemberCommandValidator()
    {
        RuleFor(c => c.HouseholdId).GreaterThan(0);
        RuleFor(c => c.PersonId).GreaterThan(0);
    }
}
