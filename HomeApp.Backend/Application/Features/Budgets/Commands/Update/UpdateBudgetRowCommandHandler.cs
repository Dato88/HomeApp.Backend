using Application.Abstractions.BudgetModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed class UpdateBudgetRowCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<UpdateBudgetRowCommandHandler> logger)
    : IRequestHandler<UpdateBudgetRowCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<UpdateBudgetRowCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateBudgetRowCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.UpdateBudgetRowAsync(request.BudgetRowId, request.Index,
            request.Title, request.CategoryId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating budgetrow failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating budgetrow: {result.Value}");

        return result;
    }
}
