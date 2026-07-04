using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed record DeleteTransactionCommand(int TransactionId) : IRequest<Result<int>>;
