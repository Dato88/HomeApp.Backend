using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Logger
{
    public partial class BudgetLoggerExtension<T>(ILogger<T> logger)
    {
        private readonly ILogger<T> _logger = logger;


        [LoggerMessage(Message = "{when} => Creating budgetCell: {budgetCell}", Level = LogLevel.Information, EventId = 1)]
        public partial void LogBudgetCell(BudgetCell budgetCell, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetColumn: {budgetColumn}", Level = LogLevel.Information, EventId = 2)]
        public partial void LogBudgetColumn(BudgetColumn budgetColumn, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetGroup: {budgetGroup}", Level = LogLevel.Information, EventId = 3)]
        public partial void LogBudgetGroup(BudgetGroup budgetGroup, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetRow: {budgetRow}", Level = LogLevel.Information, EventId = 4)]
        public partial void LogBudgetRow(BudgetRow budgetRow, DateTime when);


        [LoggerMessage(Message = "{when} => Get budget failed: {ex}", Level = LogLevel.Error, EventId = 0)]
        public partial void LogBudgetError(Exception ex, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetCell failed: {budgetCell}", Level = LogLevel.Error, EventId = 1)]
        public partial void LogBudgetCellError(BudgetCell budgetCell, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetColumn failed: {budgetColumn}", Level = LogLevel.Error, EventId = 2)]
        public partial void LogBudgetColumnError(BudgetColumn budgetColumn, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetGroup failed: {budgetGroup}", Level = LogLevel.Error, EventId = 3)]
        public partial void LogBudgetGroupError(BudgetGroup budgetGroup, DateTime when);

        [LoggerMessage(Message = "{when} => Creating budgetRow failed: {budgetRow}", Level = LogLevel.Error, EventId = 4)]
        public partial void LogBudgetRowError(BudgetRow budgetRow, DateTime when);


    }
}
