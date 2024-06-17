using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Logger
{
    public partial class BudgetLoggerExtension<T>(ILogger<T> logger)
    {
        private readonly ILogger<T> _logger = logger;

        #region Creating Information Logs
        [LoggerMessage(Message = "{when} => Creating budgetCell: {budgetCell}", Level = LogLevel.Information, EventId = 1)]
        public partial void CreateLogBudgetCellInformation(BudgetCell budgetCell, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetColumn: {budgetColumn}", Level = LogLevel.Information, EventId = 2)]
        public partial void CreateLogBudgetColumnInformation(BudgetColumn budgetColumn, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetGroup: {budgetGroup}", Level = LogLevel.Information, EventId = 3)]
        public partial void CreateLogBudgetGroupInformation(BudgetGroup budgetGroup, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetRow: {budgetRow}", Level = LogLevel.Information, EventId = 4)]
        public partial void CreateLogBudgetRowInformation(BudgetRow budgetRow, DateTime when);
        #endregion Creating Information Logs

        #region Updating Information Logs
        [LoggerMessage(Message = "{when} => Updating budgetCell: {budgetCell}", Level = LogLevel.Information, EventId = 1)]
        public partial void UpdateLogBudgetCellInformation(BudgetCell budgetCell, DateTime when);

        [LoggerMessage(Message = "{when} => Updating budgetColumn: {budgetColumn}", Level = LogLevel.Information, EventId = 2)]
        public partial void UpdateLogBudgetColumnInformation(BudgetColumn budgetColumn, DateTime when);

        [LoggerMessage(Message = "{when} => Updating budgetGroup: {budgetGroup}", Level = LogLevel.Information, EventId = 3)]
        public partial void UpdateLogBudgetGroupInformation(BudgetGroup budgetGroup, DateTime when);

        [LoggerMessage(Message = "{when} => Updating budgetRow: {budgetRow}", Level = LogLevel.Information, EventId = 4)]
        public partial void UpdateLogBudgetRowInformation(BudgetRow budgetRow, DateTime when);
        #endregion Updating Information Logs
        
        #region Delete Information Logs
        [LoggerMessage(Message = "{when} => Deleted budgetCell: {id}", Level = LogLevel.Information, EventId = 1)]
        public partial void DeleteLogBudgetCellInformation(int id, DateTime when);

        [LoggerMessage(Message = "{when} => Deleted budgetColumn: {id}", Level = LogLevel.Information, EventId = 2)]
        public partial void DeleteLogBudgetColumnInformation(int id, DateTime when);

        [LoggerMessage(Message = "{when} => Deleted budgetGroup: {id}", Level = LogLevel.Information, EventId = 3)]
        public partial void DeleteLogBudgetGroupInformation(int id, DateTime when);

        [LoggerMessage(Message = "{when} => Deleted budgetRow: {id}", Level = LogLevel.Information, EventId = 4)]
        public partial void DeleteLogBudgetRowInformation(int id, DateTime when);
        #endregion Delete Information Logs

        [LoggerMessage(Message = "{when} => Get budget failed: {ex}", Level = LogLevel.Error, EventId = 0)]
        public partial void LogBudgetError(Exception ex, DateTime when);

        #region Creating Error Logs
        [LoggerMessage(Message = "{when} => Creating budgetCell failed: {budgetCell}", Level = LogLevel.Error, EventId = 1)]
        public partial void CreateLogBudgetCellError(BudgetCell budgetCell, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetColumn failed: {budgetColumn}", Level = LogLevel.Error, EventId = 2)]
        public partial void CreateLogBudgetColumnError(BudgetColumn budgetColumn, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetGroup failed: {budgetGroup}", Level = LogLevel.Error, EventId = 3)]
        public partial void CreateLogBudgetGroupError(BudgetGroup budgetGroup, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetRow failed: {budgetRow}", Level = LogLevel.Error, EventId = 4)]
        public partial void CreateLogBudgetRowError(BudgetRow budgetRow, DateTime when);
        #endregion Creating Error Logs

        #region Updating Error Logs
        [LoggerMessage(Message = "{when} => Updating budgetCell failed: {budgetCell}", Level = LogLevel.Error, EventId = 1)]
        public partial void UpdateLogBudgetCellError(BudgetCell budgetCell, DateTime when);

        [LoggerMessage(Message = "{when} => Updating budgetColumn failed: {budgetColumn}", Level = LogLevel.Error, EventId = 2)]
        public partial void UpdateLogBudgetColumnError(BudgetColumn budgetColumn, DateTime when);

        [LoggerMessage(Message = "{when} => Updating budgetGroup failed: {budgetGroup}", Level = LogLevel.Error, EventId = 3)]
        public partial void UpdateLogBudgetGroupError(BudgetGroup budgetGroup, DateTime when);

        [LoggerMessage(Message = "{when} => Updating budgetRow failed: {budgetRow}", Level = LogLevel.Error, EventId = 4)]
        public partial void UpdateLogBudgetRowError(BudgetRow budgetRow, DateTime when);
        #endregion Updating Error Logs

        #region Delete Error Logs
        [LoggerMessage(Message = "{when} => Deleting budgetCell failed: {id}", Level = LogLevel.Error, EventId = 1)]
        public partial void DeleteLogBudgetCellError(int id, DateTime when);

        [LoggerMessage(Message = "{when} => Deleting budgetColumn failed: {id}", Level = LogLevel.Error, EventId = 2)]
        public partial void DeleteLogBudgetColumnError(int id, DateTime when);

        [LoggerMessage(Message = "{when} => Deleting budgetGroup failed: {id}", Level = LogLevel.Error, EventId = 3)]
        public partial void DeleteLogBudgetGroupError(int id, DateTime when);

        [LoggerMessage(Message = "{when} => Deleting budgetRow failed: {id}", Level = LogLevel.Error, EventId = 4)]
        public partial void DeleteLogBudgetRowError(int id, DateTime when);
        #endregion Updating Error Logs
    }
}
