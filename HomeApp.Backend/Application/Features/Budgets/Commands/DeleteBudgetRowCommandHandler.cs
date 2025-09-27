using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public sealed class DeleteBudgetRowCommandHandler(
    IBudgetCommands budgetCommands,
    IAppLogger<DeleteBudgetRowCommandHandler> logger)
    : IRequestHandler<DeleteBudgetRowCommand, Result<int>>
{
    private readonly IBudgetCommands _budgetCommands = budgetCommands;
    private readonly IAppLogger<DeleteBudgetRowCommandHandler> _logger = logger;

    public async Task<Result<int>> Handle(DeleteBudgetRowCommand request, CancellationToken cancellationToken)
    {
        var result = await _budgetCommands.DeleteBudgetRowAsync(request.BudgetRowId, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning($"Deleting budgetRow failed: {result.Error.Description} ({result.Error.Code})");

            return Result.Failure<int>(result.Error);
        }

        _logger.LogInformation($"Deleting budgetRow: {result.Value}");

        return result;
    }
}
