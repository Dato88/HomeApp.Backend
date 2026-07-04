using Application.Abstractions.BudgetModule;
using Application.Abstractions.Logging;
using Application.Features.Budgets.DTOs;
using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Queries;

public sealed class GetBudgetQueryHandler(
    IBudgetQueries budgetQueries,
    IAppLogger<GetBudgetQueryHandler> logger)
    : IRequestHandler<GetBudgetQuery, Result<BudgetResponse>>
{
    private readonly IBudgetQueries _budgetQueries = budgetQueries;
    private readonly IAppLogger<GetBudgetQueryHandler> _logger = logger;

    public async Task<Result<BudgetResponse>> Handle(GetBudgetQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var budgetResult = await _budgetQueries.GetBudgetAsync(request.HouseholdId, request.Year,
                cancellationToken);

            if (budgetResult.Error.Type == ErrorType.NotFound)
                return Result.Failure<BudgetResponse>(BudgetErrors.NotFoundAll);

            var result = (BudgetResponse)budgetResult.Value;

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get budgets failed: {ex}");

            return Result.Failure<BudgetResponse>(BudgetErrors.UnexpectedError(ex.Message));
        }
    }
}
