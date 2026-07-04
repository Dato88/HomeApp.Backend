using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Finance.Dtos;
using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Queries;

public sealed class GetCategoriesQueryHandler(
    ICategoryQueries categoryQueries,
    IAppLogger<GetCategoriesQueryHandler> logger)
    : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly ICategoryQueries _categoryQueries = categoryQueries;
    private readonly IAppLogger<GetCategoriesQueryHandler> _logger = logger;

    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _categoryQueries.GetCategoriesAsync(request.HouseholdId, cancellationToken);

            if (result.IsFailure)
                return Result.Failure<List<CategoryDto>>(result.Error);

            var response = result.Value.Select(c => (CategoryDto)c).ToList();

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get categories failed: {ex}");

            return Result.Failure<List<CategoryDto>>(FinanceErrors.UnexpectedError(ex.Message));
        }
    }
}
