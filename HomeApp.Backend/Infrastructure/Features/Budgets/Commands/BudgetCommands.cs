using Application.Abstractions.Authentication;
using Application.Features.Budgets.Commands;
using Domain.Entities.Budgets;
using Infrastructure.Database;
using SharedKernel;

namespace Infrastructure.Features.Budgets.Commands;

public sealed class BudgetCommands(HomeAppContext dbContext, IUserContext userContext) : IBudgetCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<int>> CreateBudgetAsync(int year, CancellationToken cancellationToken)
    {
        var budgetYearExists = _dbContext.Budgets.Any(x =>
            x.Year == year && x.PersonId == _userContext.PersonId);

        if (budgetYearExists)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("Budget Year already exists"));

        var newBudget = new Budget() { PersonId = _userContext.PersonId, Year = year, };

        _dbContext.Budgets.Add(newBudget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(newBudget.BudgetId);
    }

    public async Task<Result<int>> CreateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken)
    {
        var budgetIdIsValid = _dbContext.Budgets.Any(x =>
            x.BudgetId == budgetGroup.BudgetId && x.PersonId == _userContext.PersonId);

        if (!budgetIdIsValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("BudgetId is invalid"));

        _dbContext.BudgetGroups.Add(budgetGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetGroup.BudgetGroupId);
    }

    public async Task<Result<int>> CreateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken)
    {
        var budgetGroupIdIsValid = _dbContext.BudgetGroups.Any(x =>
            x.BudgetGroupId == budgetRow.BudgetGroupId && x.Budget.PersonId == _userContext.PersonId);

        if (!budgetGroupIdIsValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("BudgetGroupId is invalid"));

        _dbContext.BudgetRows.Add(budgetRow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetRow.BudgetRowId);
    }
}
