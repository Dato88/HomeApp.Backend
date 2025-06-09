using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Logger;

internal static partial class LoggerExtension
{
    // Internal LoggerMessage methods (must be private static partial)
    [LoggerMessage(EventId = 0, Level = LogLevel.Trace, Message = "{when} => {message}")]
    static partial void LogTraceInternal(this ILogger logger, string message, string when);

    [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "{when} => {message}")]
    static partial void LogDebugInternal(this ILogger logger, string message, string when);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "{when} => {message}")]
    static partial void LogInformationInternal(this ILogger logger, string message, string when);

    [LoggerMessage(EventId = 3, Level = LogLevel.Warning, Message = "{when} => {message}")]
    static partial void LogWarningInternal(this ILogger logger, string message, string when);

    [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "{when} => {message}")]
    static partial void LogErrorInternal(this ILogger logger, string message, string when);

    [LoggerMessage(EventId = 5, Level = LogLevel.Critical, Message = "{when} => {message}")]
    static partial void LogCriticalInternal(this ILogger logger, string message, string when);

    // Public wrapper methods to expose to external usage
    public static void LogTraceFormatted(this ILogger logger, string message) =>
        logger.LogTraceInternal(message, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

    public static void LogDebugFormatted(this ILogger logger, string message) =>
        logger.LogDebugInternal(message, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

    public static void LogInformationFormatted(this ILogger logger, string message) =>
        logger.LogInformationInternal(message, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

    public static void LogWarningFormatted(this ILogger logger, string message) =>
        logger.LogWarningInternal(message, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

    public static void LogErrorFormatted(this ILogger logger, string message) =>
        logger.LogErrorInternal(message, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

    public static void LogCriticalFormatted(this ILogger logger, string message) =>
        logger.LogCriticalInternal(message, DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));
}
