using FluentValidation;

namespace Application.Features.Finance.Accounts.Commands;

internal sealed class ShareAccountCommandValidator : AbstractValidator<ShareAccountCommand>
{
    public ShareAccountCommandValidator()
    {
        RuleFor(c => c.AccountId).GreaterThan(0);
        RuleFor(c => c.HouseholdId).GreaterThan(0);
    }
}
