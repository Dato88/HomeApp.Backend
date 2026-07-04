using Application.Features.Finance.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Queries;

public sealed record GetTransactionsQuery(
    int AccountId,
    DateOnly? From,
    DateOnly? To,
    int? CategoryId,
    bool? Uncategorized,
    int Page = 1,
    int PageSize = 50) : IRequest<Result<TransactionListResponse>>;
