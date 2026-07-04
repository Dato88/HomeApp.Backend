using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Accounts.Commands;

public sealed class CreateAccountCommandHandler(
    IAccountCommands accountCommands,
    IAppLogger<CreateAccountCommandHandler> logger)
    : IRequestHandler<CreateAccountCommand, Result<int>>
{
    private readonly IAccountCommands _accountCommands = accountCommands;
    private readonly IAppLogger<CreateAccountCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = await _accountCommands.CreateAccountAsync((Account)request,
            request.HouseholdIds ?? [], cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating account failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating account: {result.Value}");

        return result;
    }
}
