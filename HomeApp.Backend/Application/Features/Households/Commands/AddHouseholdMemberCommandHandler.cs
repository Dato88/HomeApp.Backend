using Application.Abstractions.HouseholdModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed class AddHouseholdMemberCommandHandler(
    IHouseholdCommands householdCommands,
    IAppLogger<AddHouseholdMemberCommandHandler> logger)
    : IRequestHandler<AddHouseholdMemberCommand, Result<int>>
{
    private readonly IHouseholdCommands _householdCommands = householdCommands;
    private readonly IAppLogger<AddHouseholdMemberCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(AddHouseholdMemberCommand request, CancellationToken cancellationToken)
    {
        var result = await _householdCommands.AddHouseholdMemberAsync(request.HouseholdId, request.Email,
            request.PersonId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Adding household member failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Adding household member: {result.Value}");

        return result;
    }
}
