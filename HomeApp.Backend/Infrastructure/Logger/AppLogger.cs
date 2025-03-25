using Application.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class AppLogger<T>(ILogger<T> logger) : IAppLogger<T>
{
    private readonly LoggerExtension<T> _loggerExt = new(logger);

    public void LogTrace(string message) => _loggerExt.LogTrace(message, DateTime.UtcNow);
    public void LogDebug(string message) => _loggerExt.LogDebug(message, DateTime.UtcNow);
    public void LogInformation(string message) => _loggerExt.LogInformation(message, DateTime.UtcNow);
    public void LogWarning(string message) => _loggerExt.LogWarning(message, DateTime.UtcNow);
    public void LogError(string message) => _loggerExt.LogError(message, DateTime.UtcNow);
    public void LogCritical(string message) => _loggerExt.LogCritical(message, DateTime.UtcNow);
    public void LogException(string message) => _loggerExt.LogException(message, DateTime.UtcNow);
}
