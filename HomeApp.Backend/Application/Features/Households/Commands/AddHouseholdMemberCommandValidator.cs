using FluentValidation;

namespace Application.Features.Households.Commands;

internal sealed class AddHouseholdMemberCommandValidator : AbstractValidator<AddHouseholdMemberCommand>
{
    public AddHouseholdMemberCommandValidator()
    {
        RuleFor(c => c.HouseholdId).GreaterThan(0);

        RuleFor(c => c)
            .Must(c => !string.IsNullOrWhiteSpace(c.Email) ^ c.PersonId.HasValue)
            .WithMessage("Exactly one of Email or PersonId must be provided.");

        RuleFor(c => c.Email)
            .EmailAddress()
            .When(c => !string.IsNullOrWhiteSpace(c.Email));

        RuleFor(c => c.PersonId)
            .GreaterThan(0)
            .When(c => c.PersonId.HasValue);
    }
}
