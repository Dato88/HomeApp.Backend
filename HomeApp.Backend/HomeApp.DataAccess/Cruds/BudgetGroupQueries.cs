using System.Linq.Expressions;
using HomeApp.DataAccess.Cruds.Interfaces;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Validations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.DataAccess.Cruds;

public class BudgetGroupQueries(HomeAppContext dbContext, IBudgetValidation budgetValidation)
    : BaseQueriesOld<BudgetGroup>(dbContext), IBudgetGroupCrud
{
    private readonly IBudgetValidation _budgetValidation = budgetValidation;

    public override async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetGroup.Id));

        var budgetGroup = await DbContext.BudgetGroups.FindAsync(id, cancellationToken);

        if (budgetGroup == null)
            throw new InvalidOperationException(BudgetMessage.GroupNotFound);

        DbContext.BudgetGroups.Remove(budgetGroup);
        await DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

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

        DbContext.BudgetGroups.Add(budgetGroup);
        await DbContext.SaveChangesAsync(cancellationToken);

        return budgetGroup;
    }

    public override async Task<BudgetGroup> FindByIdAsync(int id, CancellationToken cancellationToken,
        bool asNoTracking = true,
        params string[] includes)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetGroup.Id));

        var query = DbContext.BudgetGroups.AsQueryable();

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
        var query = DbContext.BudgetGroups.AsQueryable();

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
        var query = DbContext.BudgetGroups.AsQueryable();

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

        var existingBudgetGroup = await DbContext.BudgetGroups.FindAsync(budgetGroup.Id, cancellationToken);

        if (existingBudgetGroup == null)
            throw new InvalidOperationException(BudgetMessage.GroupNotFound);

        existingBudgetGroup.Index = budgetGroup.Index;
        existingBudgetGroup.Name = budgetGroup.Name;

        DbContext.BudgetGroups.Update(existingBudgetGroup);
        await DbContext.SaveChangesAsync(cancellationToken);
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
