using Application.Abstractions.Authentication;
using Application.Abstractions.BudgetModule;
using Domain.Entities.Budgets;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
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

        var newBudget = new Budget()
        {
            PersonId = _userContext.PersonId, Year = year, CreatedById = _userContext.PersonId
        };

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

        budgetGroup.CreatedById = _userContext.PersonId;

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

        budgetRow.CreatedById = _userContext.PersonId;

        _dbContext.BudgetRows.Add(budgetRow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetRow.BudgetRowId);
    }

    public async Task<Result<int>> CreateBudgetCellAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        var monthIsInValid = budgetCell.Month < 1 || budgetCell.Month > 12;
        if (monthIsInValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("Month should be between 1 and 12"));

        var budgetRowdIsValid = _dbContext.BudgetRows.Any(x =>
            x.BudgetRowId == budgetCell.BudgetRowId &&
            x.BudgetGroup.Budget.PersonId == _userContext.PersonId);

        if (!budgetRowdIsValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("BudgetRowId is invalid"));

        budgetCell.CreatedById = _userContext.PersonId;

        _dbContext.BudgetCells.Add(budgetCell);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetCell.BudgetCellId);
    }

    public async Task<Result<int>> DeleteBudgetAsync(int budgetId, CancellationToken cancellationToken)
    {
        var budget = await _dbContext.Budgets.SingleOrDefaultAsync(x =>
            x.BudgetId == budgetId &&
            x.PersonId == _userContext.PersonId);

        if (budget == null)
            return Result.Failure<int>(BudgetErrors.DeleteFailed(budgetId));

        _dbContext.Budgets.Remove(budget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetId);
    }

    public async Task<Result<int>> DeleteBudgetGroupAsync(int budgetGroupId, CancellationToken cancellationToken)
    {
        var budgetGroup = await _dbContext.BudgetGroups.SingleOrDefaultAsync(x =>
            x.BudgetGroupId == budgetGroupId &&
            x.Budget.PersonId == _userContext.PersonId);

        if (budgetGroup == null)
            return Result.Failure<int>(BudgetErrors.DeleteGroupFailed(budgetGroupId));

        _dbContext.BudgetGroups.Remove(budgetGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetGroupId);
    }

    public async Task<Result<int>> DeleteBudgetRowAsync(int budgetRowId, CancellationToken cancellationToken)
    {
        var budgetRow = await _dbContext.BudgetRows.SingleOrDefaultAsync(x =>
            x.BudgetRowId == budgetRowId &&
            x.BudgetGroup.Budget.PersonId == _userContext.PersonId);

        if (budgetRow == null)
            return Result.Failure<int>(BudgetErrors.DeleteRowFailed(budgetRowId));

        _dbContext.BudgetRows.Remove(budgetRow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetRowId);
    }

    public async Task<Result<int>> DeleteBudgetCellAsync(int budgetCellId, CancellationToken cancellationToken)
    {
        var budgetCell = await _dbContext.BudgetCells.SingleOrDefaultAsync(x =>
            x.BudgetCellId == budgetCellId &&
            x.BudgetRow.BudgetGroup.Budget.PersonId == _userContext.PersonId);

        if (budgetCell == null)
            return Result.Failure<int>(BudgetErrors.DeleteCellFailed(budgetCellId));

        _dbContext.BudgetCells.Remove(budgetCell);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetCellId);
    }

    public async Task<Result<int>> UpdateBudgetAsync(int budgetId, int year, CancellationToken cancellationToken)
    {
        var budgetYearExists = _dbContext.Budgets.Any(x =>
            x.Year == year && x.PersonId == _userContext.PersonId);

        if (budgetYearExists)
            return Result.Failure<int>(BudgetErrors.UpdateFailedWithMessage("Budget Year already exists"));

        var budget =
            _dbContext.Budgets.SingleOrDefault(x => x.BudgetId == budgetId && x.PersonId == _userContext.PersonId);

        if (budget == null)
            return Result.Failure<int>(BudgetErrors.UpdateFailedWithMessage("BudgetId is invalid"));

        budget.Year = year;
        budget.UpdatedById = _userContext.PersonId;
        budget.UpdatedAt = DateTime.UtcNow;

        _dbContext.Budgets.Update(budget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budget.BudgetId);
    }
}
