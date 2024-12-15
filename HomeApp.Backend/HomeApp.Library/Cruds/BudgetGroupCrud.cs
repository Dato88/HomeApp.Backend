using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds;

public class BudgetGroupCrud(HomeAppContext context, IBudgetValidation budgetValidation)
    : BaseCrud<BudgetGroup>(context, budgetValidation), IBudgetGroupCrud
{
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

    public override async Task<BudgetGroup> FindByIdAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetGroup.Id));

        var budgetGroup = await _context.BudgetGroups.FindAsync(id, cancellationToken);

        if (budgetGroup == null)
            throw new InvalidOperationException(BudgetMessage.GroupNotFound);

        return budgetGroup;
    }

    public override async Task<IEnumerable<BudgetGroup>> GetAllAsync(CancellationToken cancellationToken) =>
        await _context.BudgetGroups.ToListAsync(cancellationToken);

    public async Task<IEnumerable<BudgetGroup>> GetAllAsync(int userId, CancellationToken cancellationToken) =>
        await _context.BudgetGroups.Where(x => x.PersonId == userId).ToListAsync(cancellationToken);

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
}
