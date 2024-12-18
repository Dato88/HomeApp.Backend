using System.Linq.Expressions;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class BudgetColumnCrud(HomeAppContext context, IBudgetValidation budgetValidation)
    : BaseCrud<BudgetColumn>(context), IBudgetColumnCrud
{
    private readonly IBudgetValidation _budgetValidation = budgetValidation;

    public override async Task<BudgetColumn> CreateAsync(BudgetColumn budgetColumn,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetColumn);

        await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetColumn.Id, cancellationToken);
        await _budgetValidation.ValidateForEmptyStringAsync(budgetColumn.Name);
        await _budgetValidation.ValidateForPositiveIndexAsync(budgetColumn.Index);
        await _budgetValidation.ValidateBudgetColumnIndexAndNameExistsAsync(budgetColumn.Index, budgetColumn.Name,
            cancellationToken);

        _context.BudgetColumns.Add(budgetColumn);
        await _context.SaveChangesAsync(cancellationToken);

        return budgetColumn;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetColumn.Id));

        var budgetColumn = await _context.BudgetColumns.FindAsync(id, cancellationToken);

        if (budgetColumn == null)
            throw new InvalidOperationException(BudgetMessage.ColumnNotFound);

        _context.BudgetColumns.Remove(budgetColumn);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<BudgetColumn> FindByIdAsync(int id, CancellationToken cancellationToken,
        params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetColumn.Id));

        var query = _context.BudgetColumns.AsQueryable();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var budgetColumn = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (budgetColumn == null)
            throw new InvalidOperationException(BudgetMessage.ColumnNotFound);

        return budgetColumn;
    }

    public override async Task<IEnumerable<BudgetColumn>> GetAllAsync(CancellationToken cancellationToken,
        params string[] includes)
    {
        var query = _context.BudgetColumns.AsQueryable();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetColumn);

        await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumn.Id, cancellationToken);
        await _budgetValidation.ValidateForEmptyStringAsync(budgetColumn.Name);
        await _budgetValidation.ValidateForPositiveIndexAsync(budgetColumn.Index);
        await _budgetValidation.ValidateBudgetColumnIndexAndNameExistsAsync(budgetColumn.Index, budgetColumn.Name,
            cancellationToken);

        var existingBudgetColumn =
            await _context.BudgetColumns.FindAsync(budgetColumn.Id, cancellationToken);

        if (existingBudgetColumn == null)
            throw new InvalidOperationException(BudgetMessage.ColumnNotFound);

        existingBudgetColumn.Index = budgetColumn.Index;
        existingBudgetColumn.Name = budgetColumn.Name;

        _context.BudgetColumns.Update(existingBudgetColumn);
        await _context.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<BudgetColumn> ApplyIncludes(IQueryable<BudgetColumn> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<BudgetColumn, object>>>
        {
            { nameof(BudgetColumn.BudgetCells), x => x.BudgetCells }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
