using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed class UpdateAccountCommandHandler(
    IAccountCommands accountCommands,
    IAppLogger<UpdateAccountCommandHandler> logger)
    : IRequestHandler<UpdateAccountCommand, Result<int>>
{
    private readonly IAccountCommands _accountCommands = accountCommands;
    private readonly IAppLogger<UpdateAccountCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _accountCommands.UpdateAccountAsync((Account)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating account failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating account: {result.Value}");

        return result;
    }
}
