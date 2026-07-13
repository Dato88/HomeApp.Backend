using FluentValidation;

namespace Application.Features.Finance.Transactions.Commands;

internal sealed class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(c => c.TransactionId).GreaterThan(0);
        RuleFor(c => c.BookingDate).NotEmpty();
        RuleFor(c => c.PaymentPartnerName).MaximumLength(200);
        RuleFor(c => c.PaymentPartnerIban).MaximumLength(34);
        RuleFor(c => c.Purpose).MaximumLength(500);
    }
}
