namespace HomeApp.Library.Crud
{
    public class BudgetCellCrud(HomeAppContext context, IBudgetValidation budgetValidation) : BaseCrud<BudgetCell>(context, budgetValidation)
    {
        public override async Task<BudgetCell> CreateAsync(BudgetCell budgetCell)
        {
            ArgumentNullException.ThrowIfNull(budgetCell);

            await _budgetValidation.ValidateForUserIdAsync(budgetCell.UserId);
            await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetCell.BudgetRowId);
            await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetCell.BudgetColumnId);
            await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetCell.BudgetGroupId);

            _context.BudgetCells.Add(budgetCell);
            await _context.SaveChangesAsync();

            return budgetCell;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            BudgetCell? budgetCell = await _context.BudgetCells.FindAsync(id);

            if (budgetCell == null)
                throw new InvalidOperationException(BudgetMessage.CellNotFound);

            _context.BudgetCells.Remove(budgetCell);
            await _context.SaveChangesAsync();

            return true;
        }

        public override async Task<BudgetCell> FindByIdAsync(int id)
        {
            BudgetCell? budgetCell = await _context.BudgetCells.FindAsync(id);

            if (budgetCell == null)
                throw new InvalidOperationException(BudgetMessage.CellNotFound);

            return budgetCell;
        }

        public override async Task<IEnumerable<BudgetCell>> GetAllAsync()
        {
            return await _context.BudgetCells.ToListAsync();
        }

        public async Task<IEnumerable<BudgetCell>> GetAllAsync(int userId)
        {
            return await _context.BudgetCells.Where(x => x.UserId == userId).ToListAsync();
        }

        public override async Task UpdateAsync(BudgetCell budgetCell)
        {
            ArgumentNullException.ThrowIfNull(budgetCell);

            await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(budgetCell.BudgetRowId, budgetCell.UserId);
            await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetCell.BudgetRowId);
            await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetCell.BudgetColumnId);
            await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetCell.BudgetGroupId);

            BudgetCell? existingBudgetCell = await _context.BudgetCells.FindAsync(budgetCell.Id);

            if (existingBudgetCell == null)
                throw new InvalidOperationException(BudgetMessage.CellNotFound);

            existingBudgetCell.Name = budgetCell.Name;
            existingBudgetCell.BudgetRowId = budgetCell.BudgetRowId;
            existingBudgetCell.BudgetColumnId = budgetCell.BudgetColumnId;
            existingBudgetCell.BudgetGroupId = budgetCell.BudgetGroupId;

            _context.BudgetCells.Update(existingBudgetCell);
            await _context.SaveChangesAsync();
        }
    }
}
