using FluentValidation;

namespace Application.Features.Finance.Transactions.Commands;

internal sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(c => c.AccountId).GreaterThan(0);
        RuleFor(c => c.BookingDate).NotEmpty();
        RuleFor(c => c.CounterpartyName).MaximumLength(200);
        RuleFor(c => c.CounterpartyIban).MaximumLength(34);
        RuleFor(c => c.Purpose).MaximumLength(500);
        RuleFor(c => c.CategoryId)
            .GreaterThan(0)
            .When(c => c.CategoryId.HasValue);
    }
}
