using FluentValidation;

namespace Application.Features.Finance.Transactions.Commands;

internal sealed class SetTransactionCategoryCommandValidator : AbstractValidator<SetTransactionCategoryCommand>
{
    public SetTransactionCategoryCommandValidator()
    {
        RuleFor(c => c.TransactionIds)
            .NotEmpty()
            .WithMessage("At least one TransactionId is required.")
            .Must(ids => ids.Count <= 500)
            .WithMessage("At most 500 TransactionIds are allowed per request.");
        RuleForEach(c => c.TransactionIds).GreaterThan(0);
        RuleFor(c => c.CategoryId)
            .GreaterThan(0)
            .When(c => c.CategoryId.HasValue);
    }
}
