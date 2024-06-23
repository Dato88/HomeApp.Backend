using HomeApp.DataAccess.Models;
using HomeApp.Library.Cruds.Interfaces;

namespace HomeApp.Library.Cruds
{
    public class BudgetRowCrud(HomeAppContext context, IBudgetValidation budgetValidation) : BaseCrud<BudgetRow>(context, budgetValidation), IBudgetRowCrud
    {
        public override async Task<BudgetRow> CreateAsync(BudgetRow budgetRow)
        {
            ArgumentNullException.ThrowIfNull(budgetRow);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRow.Year, nameof(budgetRow.Year));

            await _budgetValidation.ValidateForUserIdAsync(budgetRow.UserId);
            await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRow.Id);
            await _budgetValidation.ValidateForEmptyStringAsync(budgetRow.Name);
            await _budgetValidation.ValidateForPositiveIndexAsync(budgetRow.Index);

            _context.BudgetRows.Add(budgetRow);
            await _context.SaveChangesAsync();

            return budgetRow;
        }

        public override async Task DeleteAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetRow.Id));

            BudgetRow? budgetRow = await _context.BudgetRows.FindAsync(id);
            if (budgetRow == null)
            {
                throw new InvalidOperationException(BudgetMessage.RowNotFound);
            }

            _context.BudgetRows.Remove(budgetRow);
            await _context.SaveChangesAsync();
        }

        public override async Task<BudgetRow> FindByIdAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(BudgetRow.Id));

            BudgetRow? budgetRow = await _context.BudgetRows.FindAsync(id);

            if (budgetRow == null)
                throw new InvalidOperationException(BudgetMessage.CellNotFound);

            return budgetRow;
        }

        public override async Task<IEnumerable<BudgetRow>> GetAllAsync()
        {
            return await _context.BudgetRows.ToListAsync();
        }

        public async Task<IEnumerable<BudgetRow>> GetAllAsync(int userId)
        {
            return await _context.BudgetRows.Where(x => x.UserId == userId).ToListAsync();
        }

        public override async Task UpdateAsync(BudgetRow budgetRow)
        {
            ArgumentNullException.ThrowIfNull(budgetRow);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRow.Year, nameof(budgetRow.Year));

            await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(budgetRow.Id, budgetRow.UserId);
            await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRow.Id);
            await _budgetValidation.ValidateForEmptyStringAsync(budgetRow.Name);
            await _budgetValidation.ValidateForPositiveIndexAsync(budgetRow.Index);

            BudgetRow? existingBudgetRow = await _context.BudgetRows.FindAsync(budgetRow.Id);

            if (existingBudgetRow == null)
                throw new InvalidOperationException(BudgetMessage.RowNotFound);

            existingBudgetRow.Index = budgetRow.Index;
            existingBudgetRow.Name = budgetRow.Name;

            _context.BudgetRows.Update(existingBudgetRow);

            await _context.SaveChangesAsync();
        }
    }
}
