using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Logger
{
    public partial class BudgetLoggerExtension<T>(ILogger<T> logger)
    {
        private readonly ILogger<T> _logger = logger;

        [LoggerMessage(0, LogLevel.Trace, "{when} => {message}")]
        public partial void LogTrace(string message, DateTime when);

        [LoggerMessage(1, LogLevel.Debug, "{when} => {message}")]
        public partial void LogDebug(string message, DateTime when);

        [LoggerMessage(2, LogLevel.Information, "{when} => {message}")]
        public partial void LogInformation(string message, DateTime when);

        [LoggerMessage(3, LogLevel.Warning, "{when} => {message}")]
        public partial void LogWarning(string message, DateTime when);
        
        [LoggerMessage(4, LogLevel.Error, "{when} => {message}")]
        public partial void LogError(string message, DateTime when);
        
        [LoggerMessage(5, LogLevel.Critical, "{when} => {message}")]
        public partial void LogCritical(string message, DateTime when);

        [LoggerMessage(6, LogLevel.Critical, "{when} => {message}")]
        public partial void LogException(string message, DateTime when);
    }
}
