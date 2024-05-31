using HomeApp.Library.Crud;

namespace HomeApp.Library.Validation
{
    public class BudgetValidation(HomeAppContext context) : BaseContext(context), IBudgetValidation
    {
        public async Task ValidateBudgetCellForUserIdChangeAsync(int userId, int budgetCellId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, UserMessage.UserId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetCellId, BudgetMessage.BudgetCellId);

            if (await _context.BudgetCells.AnyAsync(x => x.Id == budgetCellId && x.UserId != userId))
                throw new InvalidOperationException(BudgetMessage.UserChangeNotAllowed);
        }

        public async Task ValidateBudgetColumnIdExistsAsync(int budgetColumnId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetColumnId, BudgetMessage.BudgetColumnId);

            if (await _context.BudgetColumns.AnyAsync(column => column.Id == budgetColumnId))
                throw new InvalidOperationException(BudgetMessage.ColumnIdExist);
        }

        public async Task ValidateBudgetColumnIdExistsNotAsync(int budgetColumnId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetColumnId, BudgetMessage.BudgetColumnId);

            if (!await _context.BudgetColumns.AnyAsync(column => column.Id == budgetColumnId))
                throw new InvalidOperationException(BudgetMessage.ColumnIdNotExist);
        }

        public async Task ValidateBudgetColumnIndexAndNameAlreadyExistsAsync(int index, string name)
        {
            if (await _context.BudgetColumns.AnyAsync(column => column.Index == index && column.Name.Equals(name)))
                throw new InvalidOperationException(BudgetMessage.ColumnIndexAlreadyExists);
        }

        public async Task ValidateBudgetGroupForUserIdChangeAsync(int userId, int budgetGroupId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, UserMessage.UserId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetGroupId, BudgetMessage.BudgetGroupId);

            if (await _context.BudgetGroups.AnyAsync(x => x.Id == budgetGroupId && x.UserId != userId))
                throw new InvalidOperationException(BudgetMessage.UserChangeNotAllowed);
        }

        public async Task ValidateBudgetGroupIdExistsAsync(int budgetGroupId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetGroupId, BudgetMessage.BudgetGroupId);

            if (await _context.BudgetGroups.AnyAsync(column => column.Id == budgetGroupId))
                throw new InvalidOperationException(BudgetMessage.GroupIdExist);
        }

        public async Task ValidateBudgetGroupIdExistsNotAsync(int budgetGroupId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetGroupId, BudgetMessage.BudgetGroupId);

            if (!await _context.BudgetGroups.AnyAsync(group => group.Id == budgetGroupId))
                throw new InvalidOperationException(BudgetMessage.GroupIdNotExist);
        }

        public async Task ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(int index, string name)
        {
            if (await _context.BudgetGroups.AnyAsync(column => column.Index == index && column.Name.Equals(name)))
                throw new InvalidOperationException(BudgetMessage.GroupIndexAlreadyExists);
        }

        public async Task ValidateBudgetRowForUserIdChangeAsync(int userId, int budgetRowId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, UserMessage.UserId);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRowId, BudgetMessage.BudgetRowId);

            if (await _context.BudgetRows.AnyAsync(x => x.Id == budgetRowId && x.UserId != userId))
                throw new InvalidOperationException(BudgetMessage.UserChangeNotAllowed);
        }

        public async Task ValidateBudgetRowIdExistsAsync(int budgetRowId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRowId, BudgetMessage.BudgetRowId);

            if (await _context.BudgetRows.AnyAsync(row => row.Id == budgetRowId))
                throw new InvalidOperationException(BudgetMessage.RowIdNotExist);
        }

        public async Task ValidateBudgetRowIdExistsNotAsync(int budgetRowId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(budgetRowId, BudgetMessage.BudgetRowId);

            if (!await _context.BudgetRows.AnyAsync(row => row.Id == budgetRowId))
                throw new InvalidOperationException(BudgetMessage.RowIdNotExist);
        }

        public async Task ValidateForEmptyStringAsync(string name)
        {
            await Task.Delay(0);

            ArgumentException.ThrowIfNullOrEmpty(name, "Budget Name");
        }

        public async Task ValidateForPositiveIndexAsync(int index)
        {
            await Task.Delay(0);

            ArgumentOutOfRangeException.ThrowIfNegative(index, BudgetMessage.IndexMustBePositive);
        }

        public async Task ValidateForUserIdAsync(int userId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, UserMessage.UserId);

            if (!await _context.Users.AnyAsync(user => user.Id == userId))
                throw new InvalidOperationException(UserMessage.UserNotFound);
        }
    }
}
