using HomeApp.DataAccess.Models;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class BudgetCellCrud(HomeAppContext context, IBudgetValidation budgetValidation)
    : BaseCrud<BudgetCell>(context, budgetValidation), IBudgetCellCrud
{
    public override async Task<BudgetCell> CreateAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetCell);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetCell.Year, nameof(budgetCell.Year));

        await _budgetValidation.ValidateForUserIdAsync(budgetCell.UserId, cancellationToken);
        await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetCell.BudgetRowId, cancellationToken);
        await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetCell.BudgetColumnId, cancellationToken);
        await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetCell.BudgetGroupId, cancellationToken);

        _context.BudgetCells.Add(budgetCell);
        await _context.SaveChangesAsync(cancellationToken);

        return budgetCell;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetCell.Id));

        var budgetCell = await _context.BudgetCells.FindAsync(id, cancellationToken);

        if (budgetCell == null)
            throw new InvalidOperationException(BudgetMessage.CellNotFound);

        _context.BudgetCells.Remove(budgetCell);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<BudgetCell> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetCell.Id));

        var budgetCell = await _context.BudgetCells.FindAsync(id, cancellationToken);

        if (budgetCell == null)
            throw new InvalidOperationException(BudgetMessage.CellNotFound);

        return budgetCell;
    }

    public override async Task<IEnumerable<BudgetCell>> GetAllAsync(CancellationToken cancellationToken) => await _context.BudgetCells.ToListAsync(cancellationToken);

    public async Task<IEnumerable<BudgetCell>> GetAllAsync(int userId, CancellationToken cancellationToken) => await _context.BudgetCells.Where(x => x.UserId == userId).ToListAsync(cancellationToken);

    public override async Task UpdateAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetCell);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetCell.Year, nameof(budgetCell.Year));

        await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(budgetCell.BudgetRowId, budgetCell.UserId,
            cancellationToken);
        await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetCell.BudgetRowId, cancellationToken);
        await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetCell.BudgetColumnId, cancellationToken);
        await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetCell.BudgetGroupId, cancellationToken);

        var existingBudgetCell = await _context.BudgetCells.FindAsync(budgetCell.Id, cancellationToken);

        if (existingBudgetCell == null)
            throw new InvalidOperationException(BudgetMessage.CellNotFound);

        existingBudgetCell.Name = budgetCell.Name;
        existingBudgetCell.BudgetRowId = budgetCell.BudgetRowId;
        existingBudgetCell.BudgetColumnId = budgetCell.BudgetColumnId;
        existingBudgetCell.BudgetGroupId = budgetCell.BudgetGroupId;

        _context.BudgetCells.Update(existingBudgetCell);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
