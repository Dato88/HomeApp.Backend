using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly LoggerExtension<T> _loggerExt;
    private readonly IUserContext _userContext;

    public AppLogger(ILogger<T> logger, IUserContext userContext)
    {
        _loggerExt = new LoggerExtension<T>(logger);
        _userContext = userContext;
    }

    public void LogTrace(string message) => _loggerExt.LogTrace(Format(message), DateTime.UtcNow);
    public void LogDebug(string message) => _loggerExt.LogDebug(Format(message), DateTime.UtcNow);
    public void LogInformation(string message) => _loggerExt.LogInformation(Format(message), DateTime.UtcNow);
    public void LogWarning(string message) => _loggerExt.LogWarning(Format(message), DateTime.UtcNow);
    public void LogError(string message) => _loggerExt.LogError(Format(message), DateTime.UtcNow);
    public void LogCritical(string message) => _loggerExt.LogCritical(Format(message), DateTime.UtcNow);
    public void LogException(string message) => _loggerExt.LogException(Format(message), DateTime.UtcNow);

    private string Format(string message)
    {
        var userId = _userContext.UserId;
        return $"[UserId: {userId}] {message}";
    }
}
