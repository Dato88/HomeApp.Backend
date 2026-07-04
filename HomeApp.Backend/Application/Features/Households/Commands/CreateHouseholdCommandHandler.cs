using Application.Abstractions.HouseholdModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed class CreateHouseholdCommandHandler(
    IHouseholdCommands householdCommands,
    IAppLogger<CreateHouseholdCommandHandler> logger)
    : IRequestHandler<CreateHouseholdCommand, Result<int>>
{
    private readonly IHouseholdCommands _householdCommands = householdCommands;
    private readonly IAppLogger<CreateHouseholdCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateHouseholdCommand request, CancellationToken cancellationToken)
    {
        var result = await _householdCommands.CreateHouseholdAsync(request.Name, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating household failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating household: {result.Value}");

        return result;
    }
}
