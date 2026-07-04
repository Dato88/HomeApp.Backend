using Application.Abstractions.HouseholdModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed class UpdateHouseholdCommandHandler(
    IHouseholdCommands householdCommands,
    IAppLogger<UpdateHouseholdCommandHandler> logger)
    : IRequestHandler<UpdateHouseholdCommand, Result<int>>
{
    private readonly IHouseholdCommands _householdCommands = householdCommands;
    private readonly IAppLogger<UpdateHouseholdCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateHouseholdCommand request, CancellationToken cancellationToken)
    {
        var result = await _householdCommands.UpdateHouseholdAsync(request.HouseholdId, request.Name,
            cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating household failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating household: {result.Value}");

        return result;
    }
}
