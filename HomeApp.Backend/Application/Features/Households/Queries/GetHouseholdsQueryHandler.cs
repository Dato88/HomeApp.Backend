using Application.Abstractions.HouseholdModule;
using Application.Abstractions.Logging;
using Application.Features.Households.Dtos;
using Domain.Entities.Households;
using MediatR;
using SharedKernel;

namespace Application.Features.Households.Queries;

public sealed class GetHouseholdsQueryHandler(
    IHouseholdQueries householdQueries,
    IAppLogger<GetHouseholdsQueryHandler> logger)
    : IRequestHandler<GetHouseholdsQuery, Result<List<HouseholdResponse>>>
{
    private readonly IHouseholdQueries _householdQueries = householdQueries;
    private readonly IAppLogger<GetHouseholdsQueryHandler> _logger = logger;

    public async Task<Result<List<HouseholdResponse>>> Handle(GetHouseholdsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _householdQueries.GetHouseholdsAsync(cancellationToken);

            if (result.IsFailure)
                return Result.Failure<List<HouseholdResponse>>(result.Error);

            var response = result.Value.Select(h => (HouseholdResponse)h).ToList();

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get households failed: {ex}");

            return Result.Failure<List<HouseholdResponse>>(HouseholdErrors.NotFoundAll);
        }
    }
}
