using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public static partial class LoggerExtension
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Trace, Message = "{when} => {message}")]
    static partial void LogTrace(this ILogger logger, string message, DateTime when);

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "{when} => {message}")]
    static partial void LogDebug(this ILogger logger, string message, DateTime when);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "{when} => {message}")]
    static partial void LogInformation(this ILogger logger, string message, DateTime when);

    [LoggerMessage(EventId = 3, Level = LogLevel.Warning, Message = "{when} => {message}")]
    static partial void LogWarning(this ILogger logger, string message, DateTime when);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "{when} => {message}")]
    static partial void LogError(this ILogger logger, string message, DateTime when);

    [LoggerMessage(EventId = 5, Level = LogLevel.Critical, Message = "{when} => {message}")]
    static partial void LogCritical(this ILogger logger, string message, DateTime when);
}
