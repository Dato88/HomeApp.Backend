using Application.Abstractions.BudgetModule;
using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands.Update;

public sealed class UpdateBudgetGroupCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<UpdateBudgetGroupCommandHandler> logger)
    : IRequestHandler<UpdateBudgetGroupCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<UpdateBudgetGroupCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(UpdateBudgetGroupCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.UpdateBudgetGroupAsync(request.BudgetGroupId, request.Index,
            request.Title, request.BudgetGroupType, request.TargetPercent, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Updating budgetgroup failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Updating budgetgroup: {result.Value}");

        return result;
    }
}
