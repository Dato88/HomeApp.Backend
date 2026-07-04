using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed class CreateTransactionCommandHandler(
    ITransactionCommands transactionCommands,
    IAppLogger<CreateTransactionCommandHandler> logger)
    : IRequestHandler<CreateTransactionCommand, Result<int>>
{
    private readonly ITransactionCommands _transactionCommands = transactionCommands;
    private readonly IAppLogger<CreateTransactionCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = await _transactionCommands.CreateTransactionAsync((Transaction)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating transaction failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating transaction: {result.Value}");

        return result;
    }
}
