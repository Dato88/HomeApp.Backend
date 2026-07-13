using FluentValidation;

namespace Application.Features.Finance.PaymentPartners.Commands;

internal sealed class MergePaymentPartnersCommandValidator : AbstractValidator<MergePaymentPartnersCommand>
{
    public MergePaymentPartnersCommandValidator()
    {
        RuleFor(c => c.TargetPaymentPartnerId).GreaterThan(0);
        RuleFor(c => c.SourcePaymentPartnerId).GreaterThan(0)
            .NotEqual(c => c.TargetPaymentPartnerId)
            .WithMessage("Source and target payment partner must differ.");
    }
}
