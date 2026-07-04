using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed class SetTransactionCategoryCommandHandler(
    ITransactionCommands transactionCommands,
    IAppLogger<SetTransactionCategoryCommandHandler> logger)
    : IRequestHandler<SetTransactionCategoryCommand, Result<int>>
{
    private readonly ITransactionCommands _transactionCommands = transactionCommands;
    private readonly IAppLogger<SetTransactionCategoryCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(SetTransactionCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _transactionCommands.SetTransactionCategoryAsync(request.TransactionId,
            request.CategoryId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning(
                $"Setting transaction category failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Setting transaction category: {result.Value}");

        return result;
    }
}
