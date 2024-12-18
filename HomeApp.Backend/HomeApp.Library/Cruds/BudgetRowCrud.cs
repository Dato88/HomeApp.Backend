using System.Linq.Expressions;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class BudgetRowCrud(HomeAppContext context, IBudgetValidation budgetValidation)
    : BaseCrud<BudgetRow>(context), IBudgetRowCrud
{
    private readonly IBudgetValidation _budgetValidation = budgetValidation;

    public override async Task<BudgetRow> CreateAsync(BudgetRow budgetRow, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetRow);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRow.Year, nameof(budgetRow.Year));

        await _budgetValidation.ValidateForUserIdAsync(budgetRow.PersonId, cancellationToken);
        await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRow.Id, cancellationToken);
        await _budgetValidation.ValidateForEmptyStringAsync(budgetRow.Name);
        await _budgetValidation.ValidateForPositiveIndexAsync(budgetRow.Index);

        _context.BudgetRows.Add(budgetRow);
        await _context.SaveChangesAsync(cancellationToken);

        return budgetRow;
    }

    public override async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetRow.Id));

        var budgetRow = await _context.BudgetRows.FindAsync(id, cancellationToken);
        if (budgetRow == null) throw new InvalidOperationException(BudgetMessage.RowNotFound);

        _context.BudgetRows.Remove(budgetRow);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public override async Task<BudgetRow> FindByIdAsync(int id, CancellationToken cancellationToken,
        params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetRow.Id));

        var query = _context.BudgetRows.AsQueryable();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var budgetRow = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (budgetRow == null)
            throw new InvalidOperationException(BudgetMessage.CellNotFound);

        return budgetRow;
    }

    public async Task<IEnumerable<BudgetRow>> GetAllAsync(int userId, CancellationToken cancellationToken,
        params string[] includes)
    {
        var query = _context.BudgetRows.AsQueryable();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.Where(x => x.PersonId == userId).ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<BudgetRow>> GetAllAsync(CancellationToken cancellationToken,
        params string[] includes)
    {
        var query = _context.BudgetRows.AsQueryable();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(BudgetRow budgetRow, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetRow);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRow.Year, nameof(budgetRow.Year));

        await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(budgetRow.Id, budgetRow.PersonId,
            cancellationToken);
        await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRow.Id, cancellationToken);
        await _budgetValidation.ValidateForEmptyStringAsync(budgetRow.Name);
        await _budgetValidation.ValidateForPositiveIndexAsync(budgetRow.Index);

        var existingBudgetRow = await _context.BudgetRows.FindAsync(budgetRow.Id, cancellationToken);

        if (existingBudgetRow == null)
            throw new InvalidOperationException(BudgetMessage.RowNotFound);

        existingBudgetRow.Index = budgetRow.Index;
        existingBudgetRow.Name = budgetRow.Name;

        _context.BudgetRows.Update(existingBudgetRow);

        await _context.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<BudgetRow> ApplyIncludes(IQueryable<BudgetRow> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<BudgetRow, object>>>
        {
            { nameof(BudgetGroup.BudgetCells), x => x.BudgetCells }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
