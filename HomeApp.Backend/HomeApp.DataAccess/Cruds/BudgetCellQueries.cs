using System.Linq.Expressions;
using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Cruds;

public class BudgetCellQueries(HomeAppContext dbContext, IBudgetValidation budgetValidation)
    : BaseQueriesOld<BudgetCell>(dbContext), IBudgetCellCrud
{
    private readonly IBudgetValidation _budgetValidation = budgetValidation;

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetCell.Id));

        var budgetCell = await DbContext.BudgetCells.FindAsync(id, cancellationToken);

        if (budgetCell == null)
            throw new InvalidOperationException(BudgetMessage.CellNotFound);

        DbContext.BudgetCells.Remove(budgetCell);
        await DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<BudgetCell> CreateAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetCell);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetCell.Year, nameof(budgetCell.Year));

        await _budgetValidation.ValidateForUserIdAsync(budgetCell.PersonId, cancellationToken);
        await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetCell.BudgetRowId, cancellationToken);
        await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetCell.BudgetColumnId, cancellationToken);
        await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetCell.BudgetGroupId, cancellationToken);

        DbContext.BudgetCells.Add(budgetCell);
        await DbContext.SaveChangesAsync(cancellationToken);

        return budgetCell;
    }

    public override async Task<BudgetCell> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetCell.Id));

        var query = DbContext.BudgetCells.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var budgetCell = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (budgetCell == null)
            throw new InvalidOperationException(BudgetMessage.CellNotFound);

        return budgetCell;
    }

    public async Task<IEnumerable<BudgetCell>> GetAllAsync(int userId, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = DbContext.BudgetCells.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.Where(x => x.PersonId == userId).ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<BudgetCell>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = DbContext.BudgetCells.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetCell);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetCell.Year, nameof(budgetCell.Year));

        await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(budgetCell.BudgetRowId, budgetCell.PersonId,
            cancellationToken);
        await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetCell.BudgetRowId, cancellationToken);
        await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetCell.BudgetColumnId, cancellationToken);
        await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetCell.BudgetGroupId, cancellationToken);

        var existingBudgetCell = await DbContext.BudgetCells.FindAsync(budgetCell.Id, cancellationToken);

        if (existingBudgetCell == null)
            throw new InvalidOperationException(BudgetMessage.CellNotFound);

        existingBudgetCell.Name = budgetCell.Name;
        existingBudgetCell.BudgetRowId = budgetCell.BudgetRowId;
        existingBudgetCell.BudgetColumnId = budgetCell.BudgetColumnId;
        existingBudgetCell.BudgetGroupId = budgetCell.BudgetGroupId;

        DbContext.BudgetCells.Update(existingBudgetCell);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<BudgetCell> ApplyIncludes(IQueryable<BudgetCell> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<BudgetCell, object>>>
        {
            { nameof(BudgetCell.BudgetColumn), x => x.BudgetColumn },
            { nameof(BudgetCell.BudgetGroup), x => x.BudgetGroup },
            { nameof(BudgetCell.BudgetRow), x => x.BudgetRow },
            { nameof(BudgetCell.Person), x => x.Person }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
