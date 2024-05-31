namespace HomeApp.Library.Crud
{
    public class BudgetColumnCrud(HomeAppContext context, IBudgetValidation budgetValidation) : BaseCrud<BudgetColumn>(context, budgetValidation)
    {
        public override async Task<BudgetColumn> CreateAsync(BudgetColumn budgetColumn)
        {
            ArgumentNullException.ThrowIfNull(budgetColumn);

            await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetColumn.Id);
            await _budgetValidation.ValidateForEmptyStringAsync(budgetColumn.Name);
            await _budgetValidation.ValidateForPositiveIndexAsync(budgetColumn.Index);
            await _budgetValidation.ValidateBudgetColumnIndexAndNameAlreadyExistsAsync(budgetColumn.Index, budgetColumn.Name);

            _context.BudgetColumns.Add(budgetColumn);
            await _context.SaveChangesAsync();

            return budgetColumn;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            BudgetColumn? budgetColumn = await _context.BudgetColumns.FindAsync(id);

            if (budgetColumn == null)
                throw new InvalidOperationException(BudgetMessage.ColumnNotFound);

            _context.BudgetColumns.Remove(budgetColumn);
            await _context.SaveChangesAsync();

            return true;
        }

        public override async Task<BudgetColumn> FindByIdAsync(int id)
        {
            BudgetColumn? budgetColumn = await _context.BudgetColumns.FindAsync(id);

            if (budgetColumn == null)
                throw new InvalidOperationException(BudgetMessage.ColumnNotFound);

            return budgetColumn;
        }

        public override async Task<IEnumerable<BudgetColumn>> GetAllAsync()
        {
            return await _context.BudgetColumns.ToListAsync();
        }

        public override async Task UpdateAsync(BudgetColumn budgetColumn)
        {
            ArgumentNullException.ThrowIfNull(budgetColumn);

            await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumn.Id);
            await _budgetValidation.ValidateForEmptyStringAsync(budgetColumn.Name);
            await _budgetValidation.ValidateForPositiveIndexAsync(budgetColumn.Index);
            await _budgetValidation.ValidateBudgetColumnIndexAndNameAlreadyExistsAsync(budgetColumn.Index, budgetColumn.Name);

            BudgetColumn? existingBudgetColumn = await _context.BudgetColumns.FindAsync(budgetColumn.Id);

            if (existingBudgetColumn == null)
                throw new InvalidOperationException(BudgetMessage.ColumnNotFound);

            existingBudgetColumn.Index = budgetColumn.Index;
            existingBudgetColumn.Name = budgetColumn.Name;

            _context.BudgetColumns.Update(existingBudgetColumn);
            await _context.SaveChangesAsync();
        }
    }
}
