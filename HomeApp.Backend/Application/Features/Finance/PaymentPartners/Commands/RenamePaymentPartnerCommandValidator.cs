using FluentValidation;

namespace Application.Features.Finance.PaymentPartners.Commands;

internal sealed class RenamePaymentPartnerCommandValidator : AbstractValidator<RenamePaymentPartnerCommand>
{
    public RenamePaymentPartnerCommandValidator()
    {
        RuleFor(c => c.PaymentPartnerId).GreaterThan(0);
        RuleFor(c => c.DisplayName).NotEmpty()
            .WithMessage("DisplayName is required.")
            .MaximumLength(200);
    }
}
