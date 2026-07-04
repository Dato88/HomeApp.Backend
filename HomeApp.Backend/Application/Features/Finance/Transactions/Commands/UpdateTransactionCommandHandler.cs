using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed class UpdateTransactionCommandHandler(
    ITransactionCommands transactionCommands,
    IAppLogger<UpdateTransactionCommandHandler> logger)
    : IRequestHandler<UpdateTransactionCommand, Result<int>>
{
    private readonly ITransactionCommands _transactionCommands = transactionCommands;
    private readonly IAppLogger<UpdateTransactionCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await _transactionCommands.UpdateTransactionAsync((Transaction)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating transaction failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating transaction: {result.Value}");

        return result;
    }
}
