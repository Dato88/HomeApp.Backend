using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed class DeleteAccountCommandHandler(
    IAccountCommands accountCommands,
    IAppLogger<DeleteAccountCommandHandler> logger)
    : IRequestHandler<DeleteAccountCommand, Result<int>>
{
    private readonly IAccountCommands _accountCommands = accountCommands;
    private readonly IAppLogger<DeleteAccountCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _accountCommands.DeleteAccountAsync(request.AccountId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting account failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting account: {result.Value}");

        return result;
    }
}
