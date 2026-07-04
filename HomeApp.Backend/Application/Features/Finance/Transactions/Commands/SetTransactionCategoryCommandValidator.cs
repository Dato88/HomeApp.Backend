using FluentValidation;

namespace Application.Features.Finance.Transactions.Commands;

internal sealed class SetTransactionCategoryCommandValidator : AbstractValidator<SetTransactionCategoryCommand>
{
    public SetTransactionCategoryCommandValidator()
    {
        RuleFor(c => c.TransactionId).GreaterThan(0);
        RuleFor(c => c.CategoryId)
            .GreaterThan(0)
            .When(c => c.CategoryId.HasValue);
    }
}
