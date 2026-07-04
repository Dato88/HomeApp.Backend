using Application.Abstractions.HouseholdModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed class DeleteHouseholdCommandHandler(
    IHouseholdCommands householdCommands,
    IAppLogger<DeleteHouseholdCommandHandler> logger)
    : IRequestHandler<DeleteHouseholdCommand, Result<int>>
{
    private readonly IHouseholdCommands _householdCommands = householdCommands;
    private readonly IAppLogger<DeleteHouseholdCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteHouseholdCommand request, CancellationToken cancellationToken)
    {
        var result = await _householdCommands.DeleteHouseholdAsync(request.HouseholdId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting household failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting household: {result.Value}");

        return result;
    }
}
