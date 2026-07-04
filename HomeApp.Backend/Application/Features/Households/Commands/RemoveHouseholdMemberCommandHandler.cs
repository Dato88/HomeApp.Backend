using Application.Abstractions.HouseholdModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed class RemoveHouseholdMemberCommandHandler(
    IHouseholdCommands householdCommands,
    IAppLogger<RemoveHouseholdMemberCommandHandler> logger)
    : IRequestHandler<RemoveHouseholdMemberCommand, Result<int>>
{
    private readonly IHouseholdCommands _householdCommands = householdCommands;
    private readonly IAppLogger<RemoveHouseholdMemberCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(RemoveHouseholdMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _householdCommands.RemoveHouseholdMemberAsync(request.HouseholdId, request.PersonId,
            cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Removing household member failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Removing household member: {result.Value}");

        return result;
    }
}
