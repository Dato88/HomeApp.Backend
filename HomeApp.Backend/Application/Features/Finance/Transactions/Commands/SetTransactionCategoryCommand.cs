using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

// Assigns (or clears, CategoryId = null) the category for one or many transactions in one
// all-or-nothing batch; the result is the number of updated transactions
public sealed record SetTransactionCategoryCommand(IReadOnlyList<int> TransactionIds, int? CategoryId)
    : IRequest<Result<int>>;
