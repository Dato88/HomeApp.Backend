using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Finance.Dtos;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Queries;

public sealed class GetTransactionsQueryHandler(
    ITransactionQueries transactionQueries,
    IAppLogger<GetTransactionsQueryHandler> logger)
    : IRequestHandler<GetTransactionsQuery, Result<TransactionListResponse>>
{
    private readonly ITransactionQueries _transactionQueries = transactionQueries;
    private readonly IAppLogger<GetTransactionsQueryHandler> _logger = logger;

    public async Task<Result<TransactionListResponse>> Handle(GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _transactionQueries.GetTransactionsAsync(request.AccountId, request.From,
                request.To, request.CategoryId, request.Uncategorized, request.CounterpartyIban,
                request.Page, request.PageSize, cancellationToken);

            if (result.IsFailure)
                return Result.Failure<TransactionListResponse>(result.Error);

            var response = new TransactionListResponse
            {
                TotalCount = result.Value.TotalCount,
                Transactions = result.Value.Items.Select(t => (TransactionDto)t).ToList()
            };

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get transactions failed: {ex}");

            return Result.Failure<TransactionListResponse>(FinanceErrors.UnexpectedError(ex.Message));
        }
    }
}
