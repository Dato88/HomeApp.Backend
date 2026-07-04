using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed record SetTransactionCategoryCommand(int TransactionId, int? CategoryId) : IRequest<Result<int>>;
