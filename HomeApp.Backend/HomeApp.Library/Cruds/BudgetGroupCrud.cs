using System.Linq.Expressions;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class BudgetGroupCrud(HomeAppContext context, IBudgetValidation budgetValidation)
    : BaseCrud<BudgetGroup>(context), IBudgetGroupCrud
{
    private readonly IBudgetValidation _budgetValidation = budgetValidation;

    public override async Task<BudgetGroup> CreateAsync(BudgetGroup budgetGroup,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetGroup);

        await _budgetValidation.ValidateForUserIdAsync(budgetGroup.PersonId, cancellationToken);
        await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroup.Id, cancellationToken);
        await _budgetValidation.ValidateForEmptyStringAsync(budgetGroup.Name);
        await _budgetValidation.ValidateForPositiveIndexAsync(budgetGroup.Index);
        await _budgetValidation.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(budgetGroup.Index,
            budgetGroup.Name, cancellationToken);

        _context.BudgetGroups.Add(budgetGroup);
        await _context.SaveChangesAsync(cancellationToken);

        return budgetGroup;
    }

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetGroup.Id));

        var budgetGroup = await _context.BudgetGroups.FindAsync(id, cancellationToken);

        if (budgetGroup == null)
            throw new InvalidOperationException(BudgetMessage.GroupNotFound);

        _context.BudgetGroups.Remove(budgetGroup);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public override async Task<BudgetGroup> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetGroup.Id));

        var query = _context.BudgetGroups.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        var budgetGroup = await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (budgetGroup == null)
            throw new InvalidOperationException(BudgetMessage.GroupNotFound);

        return budgetGroup;
    }

    public async Task<IEnumerable<BudgetGroup>> GetAllAsync(int userId, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = _context.BudgetGroups.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.Where(x => x.PersonId == userId).ToListAsync(cancellationToken);
    }

    public override async Task<IEnumerable<BudgetGroup>> GetAllAsync(CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        var query = _context.BudgetGroups.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is { Length: > 0 })
            query = ApplyIncludes(query, includes);

        return await query.ToListAsync(cancellationToken);
    }

    public override async Task UpdateAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budgetGroup);

        await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(budgetGroup.Id, budgetGroup.PersonId,
            cancellationToken);
        await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroup.Id, cancellationToken);
        await _budgetValidation.ValidateForEmptyStringAsync(budgetGroup.Name);
        await _budgetValidation.ValidateForPositiveIndexAsync(budgetGroup.Index);

        var existingBudgetGroup = await _context.BudgetGroups.FindAsync(budgetGroup.Id, cancellationToken);

        if (existingBudgetGroup == null)
            throw new InvalidOperationException(BudgetMessage.GroupNotFound);

        existingBudgetGroup.Index = budgetGroup.Index;
        existingBudgetGroup.Name = budgetGroup.Name;

        _context.BudgetGroups.Update(existingBudgetGroup);
        await _context.SaveChangesAsync(cancellationToken);
    }

    protected override IQueryable<BudgetGroup> ApplyIncludes(IQueryable<BudgetGroup> query, params string[] includes)
    {
        var includeMappings = new Dictionary<string, Expression<Func<BudgetGroup, object>>>
        {
            { nameof(BudgetGroup.BudgetCells), x => x.BudgetCells }
        };

        foreach (var include in includes)
            if (includeMappings.ContainsKey(include))
                query = query.Include(includeMappings[include]);

        return query;
    }
}
