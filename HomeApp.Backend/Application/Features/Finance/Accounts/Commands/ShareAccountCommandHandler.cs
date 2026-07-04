using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed class ShareAccountCommandHandler(
    IAccountCommands accountCommands,
    IAppLogger<ShareAccountCommandHandler> logger)
    : IRequestHandler<ShareAccountCommand, Result<int>>
{
    private readonly IAccountCommands _accountCommands = accountCommands;
    private readonly IAppLogger<ShareAccountCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(ShareAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _accountCommands.ShareAccountAsync(request.AccountId, request.HouseholdId,
            cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Sharing account failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Sharing account: {result.Value}");

        return result;
    }
}
