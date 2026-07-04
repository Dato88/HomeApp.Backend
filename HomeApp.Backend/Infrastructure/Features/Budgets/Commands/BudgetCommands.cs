using Application.Abstractions.Authentication;
using Application.Abstractions.BudgetModule;
using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Budgets.Commands;

public sealed class BudgetCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext) : IBudgetCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> CreateBudgetAsync(int householdId, int year, CancellationToken cancellationToken)
    {
        var isMember = _dbContext.HouseholdMembers.Any(x =>
            x.HouseholdId == householdId && x.PersonId == _executionContext.PersonId);

        if (!isMember)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("HouseholdId is invalid"));

        var budgetYearExists = _dbContext.Budgets.Any(x =>
            x.Year == year && x.HouseholdId == householdId);

        if (budgetYearExists)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("Budget Year already exists"));

        var newBudget = new Budget()
        {
            HouseholdId = householdId, Year = year, CreatedById = _executionContext.PersonId
        };

        _dbContext.Budgets.Add(newBudget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(newBudget.BudgetId);
    }

    public async Task<Result<int>> CreateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken)
    {
        var budgetIdIsValid = _dbContext.Budgets.Any(x =>
            x.BudgetId == budgetGroup.BudgetId &&
            x.Household.Members.Any(m => m.PersonId == _executionContext.PersonId));

        if (!budgetIdIsValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("BudgetId is invalid"));

        budgetGroup.CreatedById = _executionContext.PersonId;

        _dbContext.BudgetGroups.Add(budgetGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetGroup.BudgetGroupId);
    }

    public async Task<Result<int>> CreateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken)
    {
        var budgetGroupIdIsValid = _dbContext.BudgetGroups.Any(x =>
            x.BudgetGroupId == budgetRow.BudgetGroupId &&
            x.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId));

        if (!budgetGroupIdIsValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("BudgetGroupId is invalid"));

        var categoryError = await ValidateRowCategoryAsync(budgetRow.BudgetGroupId, budgetRow.CategoryId, null,
            cancellationToken);

        if (categoryError is not null)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage(categoryError));

        budgetRow.CreatedById = _executionContext.PersonId;

        _dbContext.BudgetRows.Add(budgetRow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetRow.BudgetRowId);
    }

    // Returns an error message or null. A row category must belong to the budget's household and may
    // be linked to at most one row per budget, otherwise the IST aggregation would double-count.
    private async Task<string?> ValidateRowCategoryAsync(int budgetGroupId, int? categoryId, int? budgetRowId,
        CancellationToken cancellationToken)
    {
        if (!categoryId.HasValue)
            return null;

        var budgetInfo = await _dbContext.BudgetGroups
            .Where(x => x.BudgetGroupId == budgetGroupId)
            .Select(x => new { x.BudgetId, x.Budget!.HouseholdId })
            .SingleOrDefaultAsync(cancellationToken);

        if (budgetInfo == null)
            return "BudgetGroupId is invalid";

        var categoryIsValid = _dbContext.Categories.Any(c =>
            c.CategoryId == categoryId.Value && c.HouseholdId == budgetInfo.HouseholdId);

        if (!categoryIsValid)
            return "CategoryId is invalid";

        var categoryAlreadyMapped = _dbContext.BudgetRows.Any(r =>
            r.BudgetGroup.BudgetId == budgetInfo.BudgetId &&
            r.CategoryId == categoryId.Value &&
            (!budgetRowId.HasValue || r.BudgetRowId != budgetRowId.Value));

        if (categoryAlreadyMapped)
            return "Category is already mapped to another row of this budget";

        return null;
    }

    public async Task<Result<int>> CreateBudgetCellAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        var monthIsInValid = budgetCell.Month < 1 || budgetCell.Month > 12;
        if (monthIsInValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("Month should be between 1 and 12"));

        var budgetRowdIsValid = _dbContext.BudgetRows.Any(x =>
            x.BudgetRowId == budgetCell.BudgetRowId &&
            x.BudgetGroup.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId));

        if (!budgetRowdIsValid)
            return Result.Failure<int>(BudgetErrors.CreateFailedWithMessage("BudgetRowId is invalid"));

        budgetCell.CreatedById = _executionContext.PersonId;

        _dbContext.BudgetCells.Add(budgetCell);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetCell.BudgetCellId);
    }

    public async Task<Result<int>> DeleteBudgetAsync(int budgetId, CancellationToken cancellationToken)
    {
        var budget = await _dbContext.Budgets.SingleOrDefaultAsync(x =>
            x.BudgetId == budgetId &&
            x.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

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
            x.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

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
            x.BudgetGroup.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId),
            cancellationToken);

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
            x.BudgetRow.BudgetGroup.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId),
            cancellationToken);

        if (budgetCell == null)
            return Result.Failure<int>(BudgetErrors.DeleteCellFailed(budgetCellId));

        _dbContext.BudgetCells.Remove(budgetCell);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetCellId);
    }

    public async Task<Result<int>> UpdateBudgetAsync(int budgetId, int year, CancellationToken cancellationToken)
    {
        var budget = await _dbContext.Budgets.SingleOrDefaultAsync(x =>
            x.BudgetId == budgetId &&
            x.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (budget == null)
            return Result.Failure<int>(BudgetErrors.UpdateFailedWithMessage("BudgetId is invalid"));

        var budgetYearExists = _dbContext.Budgets.Any(x =>
            x.Year == year && x.HouseholdId == budget.HouseholdId && x.BudgetId != budgetId);

        if (budgetYearExists)
            return Result.Failure<int>(BudgetErrors.UpdateFailedWithMessage("Budget Year already exists"));

        budget.Year = year;
        budget.UpdatedById = _executionContext.PersonId;
        budget.UpdatedAt = DateTime.UtcNow;

        _dbContext.Budgets.Update(budget);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budget.BudgetId);
    }

    public async Task<Result<int>> UpdateBudgetGroupAsync(int budgetGroupId, int index, string title,
        BudgetGroupType budgetGroupType, CancellationToken cancellationToken)
    {
        var budgetGroup = await _dbContext.BudgetGroups.SingleOrDefaultAsync(x =>
            x.BudgetGroupId == budgetGroupId &&
            x.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (budgetGroup == null)
            return Result.Failure<int>(BudgetErrors.UpdateGroupFailedWithMessage("BudgetGroupId is invalid"));

        var indexConflict = budgetGroup.Index != index && _dbContext.BudgetGroups.Any(x =>
            x.BudgetId == budgetGroup.BudgetId &&
            x.Index == index &&
            x.BudgetGroupId != budgetGroupId);

        if (indexConflict)
            return Result.Failure<int>(BudgetErrors.UpdateGroupFailedWithMessage("Index already exists"));

        budgetGroup.Index = index;
        budgetGroup.Title = title;
        budgetGroup.BudgetGroupType = budgetGroupType;
        budgetGroup.UpdatedById = _executionContext.PersonId;
        budgetGroup.UpdatedAt = DateTime.UtcNow;

        _dbContext.BudgetGroups.Update(budgetGroup);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetGroup.BudgetGroupId);
    }

    public async Task<Result<int>> UpdateBudgetRowAsync(int budgetRowId, int index, string title, int? categoryId,
        CancellationToken cancellationToken)
    {
        var budgetRow = await _dbContext.BudgetRows.SingleOrDefaultAsync(x =>
            x.BudgetRowId == budgetRowId &&
            x.BudgetGroup.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId),
            cancellationToken);

        if (budgetRow == null)
            return Result.Failure<int>(BudgetErrors.UpdateRowFailedWithMessage("BudgetRowId is invalid"));

        var indexConflict = budgetRow.Index != index && _dbContext.BudgetRows.Any(x =>
            x.BudgetGroupId == budgetRow.BudgetGroupId &&
            x.Index == index &&
            x.BudgetRowId != budgetRowId);

        if (indexConflict)
            return Result.Failure<int>(BudgetErrors.UpdateRowFailedWithMessage("Index already exists"));

        var categoryError = await ValidateRowCategoryAsync(budgetRow.BudgetGroupId, categoryId, budgetRowId,
            cancellationToken);

        if (categoryError is not null)
            return Result.Failure<int>(BudgetErrors.UpdateRowFailedWithMessage(categoryError));

        budgetRow.Index = index;
        budgetRow.Title = title;
        budgetRow.CategoryId = categoryId;
        budgetRow.UpdatedById = _executionContext.PersonId;
        budgetRow.UpdatedAt = DateTime.UtcNow;

        _dbContext.BudgetRows.Update(budgetRow);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetRow.BudgetRowId);
    }

    public async Task<Result<int>> UpdateBudgetCellAsync(int budgetCellId, decimal amount,
        CancellationToken cancellationToken)
    {
        var budgetCell = await _dbContext.BudgetCells.SingleOrDefaultAsync(x =>
            x.BudgetCellId == budgetCellId &&
            x.BudgetRow.BudgetGroup.Budget.Household.Members.Any(m => m.PersonId == _executionContext.PersonId),
            cancellationToken);

        if (budgetCell == null)
            return Result.Failure<int>(BudgetErrors.UpdateCellFailedWithMessage("BudgetCellId is invalid"));

        budgetCell.Amount = amount;
        budgetCell.UpdatedById = _executionContext.PersonId;
        budgetCell.UpdatedAt = DateTime.UtcNow;

        _dbContext.BudgetCells.Update(budgetCell);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(budgetCell.BudgetCellId);
    }
}
