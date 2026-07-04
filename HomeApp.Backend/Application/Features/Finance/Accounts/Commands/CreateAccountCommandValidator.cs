using Domain.ValueObjects;
using FluentValidation;

namespace Application.Features.Finance.Accounts.Commands;

internal sealed class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(150);

        RuleFor(c => c.Iban)
            .Must(iban => Iban.IsValid(iban!))
            .When(c => !string.IsNullOrWhiteSpace(c.Iban))
            .WithMessage("Iban is not valid.");

        RuleFor(c => c.Bic)
            .MaximumLength(11);

        RuleFor(c => c.CurrencyCode)
            .Length(3)
            .When(c => !string.IsNullOrWhiteSpace(c.CurrencyCode))
            .WithMessage("CurrencyCode must be a 3-letter ISO code.");

        RuleForEach(c => c.HouseholdIds)
            .GreaterThan(0)
            .When(c => c.HouseholdIds is not null);
    }
}
