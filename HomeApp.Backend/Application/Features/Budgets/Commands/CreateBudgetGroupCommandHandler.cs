using Application.Abstractions.Logging;
using Domain.Entities.Budgets;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public class CreateBudgetGroupCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<CreateBudgetGroupCommandHandler> logger)
    : IRequestHandler<CreateBudgetGroupCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<CreateBudgetGroupCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(CreateBudgetGroupCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.CreateBudgetGroupAsync((BudgetGroup)request, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Creating budgetgroup failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Creating budgetgroup: {result.Value}");

        return result;
    }
}
