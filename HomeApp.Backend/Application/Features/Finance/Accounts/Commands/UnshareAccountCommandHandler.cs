using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed class UnshareAccountCommandHandler(
    IAccountCommands accountCommands,
    IAppLogger<UnshareAccountCommandHandler> logger)
    : IRequestHandler<UnshareAccountCommand, Result<int>>
{
    private readonly IAccountCommands _accountCommands = accountCommands;
    private readonly IAppLogger<UnshareAccountCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UnshareAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _accountCommands.UnshareAccountAsync(request.AccountId, request.HouseholdId,
            cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Unsharing account failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Unsharing account: {result.Value}");

        return result;
    }
}
