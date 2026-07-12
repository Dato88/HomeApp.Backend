using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Finance.Dtos;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Queries;

public sealed class GetCategoryGroupsQueryHandler(
    ICategoryGroupQueries categoryGroupQueries,
    IAppLogger<GetCategoryGroupsQueryHandler> logger)
    : IRequestHandler<GetCategoryGroupsQuery, Result<List<CategoryGroupDto>>>
{
    private readonly ICategoryGroupQueries _categoryGroupQueries = categoryGroupQueries;
    private readonly IAppLogger<GetCategoryGroupsQueryHandler> _logger = logger;

    public async Task<Result<List<CategoryGroupDto>>> Handle(GetCategoryGroupsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _categoryGroupQueries.GetCategoryGroupsAsync(request.HouseholdId, cancellationToken);

            if (result.IsFailure)
                return Result.Failure<List<CategoryGroupDto>>(result.Error);

            var response = result.Value.Select(g => (CategoryGroupDto)g).ToList();

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get category groups failed: {ex}");

            return Result.Failure<List<CategoryGroupDto>>(FinanceErrors.UnexpectedError(ex.Message));
        }
    }
}
