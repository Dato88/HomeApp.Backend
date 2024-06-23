using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds
{
    public class BudgetGroupCrud(HomeAppContext context, IBudgetValidation budgetValidation) : BaseCrud<BudgetGroup>(context, budgetValidation), IBudgetGroupCrud
    {
        public override async Task<BudgetGroup> CreateAsync(BudgetGroup budgetGroup)
        {
            ArgumentNullException.ThrowIfNull(budgetGroup);

            await _budgetValidation.ValidateForUserIdAsync(budgetGroup.UserId);
            await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroup.Id);
            await _budgetValidation.ValidateForEmptyStringAsync(budgetGroup.Name);
            await _budgetValidation.ValidateForPositiveIndexAsync(budgetGroup.Index);
            await _budgetValidation.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(budgetGroup.Index, budgetGroup.Name);

            _context.BudgetGroups.Add(budgetGroup);
            await _context.SaveChangesAsync();

            return budgetGroup;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetGroup.Id));

            BudgetGroup? budgetGroup = await _context.BudgetGroups.FindAsync(id);

            if (budgetGroup == null)
                throw new InvalidOperationException(BudgetMessage.GroupNotFound);

            _context.BudgetGroups.Remove(budgetGroup);
            await _context.SaveChangesAsync();

            return true;
        }

        public override async Task<BudgetGroup> FindByIdAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetGroup.Id));

            BudgetGroup? budgetGroup = await _context.BudgetGroups.FindAsync(id);

            if (budgetGroup == null)
                throw new InvalidOperationException(BudgetMessage.GroupNotFound);

            return budgetGroup;
        }

        public override async Task<IEnumerable<BudgetGroup>> GetAllAsync()
        {
            return await _context.BudgetGroups.ToListAsync();
        }

        public async Task<IEnumerable<BudgetGroup>> GetAllAsync(int userId)
        {
            return await _context.BudgetGroups.Where(x => x.UserId == userId).ToListAsync();
        }

        public override async Task UpdateAsync(BudgetGroup budgetGroup)
        {
            ArgumentNullException.ThrowIfNull(budgetGroup);

            await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(budgetGroup.Id, budgetGroup.UserId);
            await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroup.Id);
            await _budgetValidation.ValidateForEmptyStringAsync(budgetGroup.Name);
            await _budgetValidation.ValidateForPositiveIndexAsync(budgetGroup.Index);

            BudgetGroup? existingBudgetGroup = await _context.BudgetGroups.FindAsync(budgetGroup.Id);

            if (existingBudgetGroup == null)
                throw new InvalidOperationException(BudgetMessage.GroupNotFound);

            existingBudgetGroup.Index = budgetGroup.Index;
            existingBudgetGroup.Name = budgetGroup.Name;

            _context.BudgetGroups.Update(existingBudgetGroup);
            await _context.SaveChangesAsync();
        }
    }
}
