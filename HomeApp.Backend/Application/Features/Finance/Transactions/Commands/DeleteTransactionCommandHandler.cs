using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed class DeleteTransactionCommandHandler(
    ITransactionCommands transactionCommands,
    IAppLogger<DeleteTransactionCommandHandler> logger)
    : IRequestHandler<DeleteTransactionCommand, Result<int>>
{
    private readonly ITransactionCommands _transactionCommands = transactionCommands;
    private readonly IAppLogger<DeleteTransactionCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await _transactionCommands.DeleteTransactionAsync(request.TransactionId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting transaction failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting transaction: {result.Value}");

        return result;
    }
}
