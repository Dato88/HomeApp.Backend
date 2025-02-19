﻿using HomeApp.DataAccess.Cruds;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Validations;

public class BudgetValidation(HomeAppContext dbContext) : BaseContext(dbContext), IBudgetValidation
{
    public async Task ValidateBudgetCellForUserIdChangeAsync(int userId, int budgetCellId,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, PersonMessage.PersonId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetCellId, BudgetMessage.BudgetCellId);

        if (await DbContext.BudgetCells.AnyAsync(x => x.Id == budgetCellId && x.PersonId != userId, cancellationToken))
            throw new InvalidOperationException(BudgetMessage.UserChangeNotAllowed);
    }

    public async Task ValidateBudgetColumnIdExistsAsync(int budgetColumnId, CancellationToken cancellationToken)
    {
        if (await DbContext.BudgetColumns.AnyAsync(column => column.Id == budgetColumnId, cancellationToken))
            throw new InvalidOperationException(BudgetMessage.ColumnIdExist);
    }

    public async Task ValidateBudgetColumnIdExistsNotAsync(int budgetColumnId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetColumnId, BudgetMessage.BudgetColumnId);

        if (!await DbContext.BudgetColumns.AnyAsync(column => column.Id == budgetColumnId, cancellationToken))
            throw new InvalidOperationException(BudgetMessage.ColumnIdNotExist);
    }

    public async Task ValidateBudgetColumnIndexAndNameExistsAsync(int index, string name,
        CancellationToken cancellationToken)
    {
        if (await DbContext.BudgetColumns.AnyAsync(column => column.Index == index && column.Name.Equals(name),
                cancellationToken))
            throw new InvalidOperationException(BudgetMessage.ColumnIndexAlreadyExists);
    }

    public async Task ValidateBudgetGroupForUserIdChangeAsync(int userId, int budgetGroupId,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, PersonMessage.PersonId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetGroupId, BudgetMessage.BudgetGroupId);

        if (await DbContext.BudgetGroups.AnyAsync(group => group.Id == budgetGroupId && group.PersonId != userId,
                cancellationToken))
            throw new InvalidOperationException(BudgetMessage.UserChangeNotAllowed);
    }

    public async Task ValidateBudgetGroupIdExistsAsync(int budgetGroupId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetGroupId, BudgetMessage.BudgetGroupId);

        if (await DbContext.BudgetGroups.AnyAsync(group => group.Id == budgetGroupId, cancellationToken))
            throw new InvalidOperationException(BudgetMessage.GroupIdExist);
    }

    public async Task ValidateBudgetGroupIdExistsNotAsync(int budgetGroupId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetGroupId, BudgetMessage.BudgetGroupId);

        if (!await DbContext.BudgetGroups.AnyAsync(group => group.Id == budgetGroupId, cancellationToken))
            throw new InvalidOperationException(BudgetMessage.GroupIdNotExist);
    }

    public async Task ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(int index, string name,
        CancellationToken cancellationToken)
    {
        if (await DbContext.BudgetGroups.AnyAsync(group => group.Index == index && group.Name.Equals(name),
                cancellationToken))
            throw new InvalidOperationException(BudgetMessage.GroupIndexAlreadyExists);
    }

    public async Task ValidateBudgetRowForUserIdChangeAsync(int userId, int budgetRowId,
        CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, PersonMessage.PersonId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRowId, BudgetMessage.BudgetRowId);

        if (await DbContext.BudgetRows.AnyAsync(row => row.Id == budgetRowId && row.PersonId != userId,
                cancellationToken))
            throw new InvalidOperationException(BudgetMessage.UserChangeNotAllowed);
    }

    public async Task ValidateBudgetRowIdExistsAsync(int budgetRowId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRowId, BudgetMessage.BudgetRowId);

        if (await DbContext.BudgetRows.AnyAsync(row => row.Id == budgetRowId, cancellationToken))
            throw new InvalidOperationException(BudgetMessage.RowIdExists);
    }

    public async Task ValidateBudgetRowIdExistsNotAsync(int budgetRowId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRowId, BudgetMessage.BudgetRowId);

        if (!await DbContext.BudgetRows.AnyAsync(row => row.Id == budgetRowId, cancellationToken))
            throw new InvalidOperationException(BudgetMessage.RowIdNotExist);
    }

    public async Task ValidateForEmptyStringAsync(string name)
    {
        await Task.Delay(0);

        ArgumentException.ThrowIfNullOrEmpty(name, "Budget Name");
    }

    public async Task ValidateForPositiveIndexAsync(int index)
    {
        await Task.Delay(0);

        ArgumentOutOfRangeException.ThrowIfNegative(index, BudgetMessage.IndexMustBePositive);
    }

    public async Task ValidateForUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, PersonMessage.PersonId);

        if (!await DbContext.People.AnyAsync(user => user.Id == userId, cancellationToken))
            throw new InvalidOperationException(PersonMessage.PersonNotFound);
    }
}
