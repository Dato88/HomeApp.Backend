using FluentValidation;

namespace Application.Features.Finance.Transactions.Queries;

internal sealed class GetTransactionsQueryValidator : AbstractValidator<GetTransactionsQuery>
{
    public GetTransactionsQueryValidator()
    {
        RuleFor(c => c.AccountId).GreaterThan(0);
        RuleFor(c => c.CounterpartyIban)
            .MaximumLength(50)
            .When(c => c.CounterpartyIban != null);
        RuleFor(c => c.Page).GreaterThan(0);
        RuleFor(c => c.PageSize).InclusiveBetween(1, 200);
    }
}
