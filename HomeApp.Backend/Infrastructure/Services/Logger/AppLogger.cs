using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Logger;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<T> _logger;
    private readonly IUserContext _userContext;

    public AppLogger(ILogger<T> logger, IUserContext userContext, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userContext = userContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public void LogTrace(string message) => _logger.LogTrace(message, DateTime.UtcNow);
    public void LogDebug(string message) => _logger.LogDebug(message, DateTime.UtcNow);
    public void LogInformation(string message) => _logger.LogInformation(message, DateTime.UtcNow);
    public void LogWarning(string message) => _logger.LogWarning(message, DateTime.UtcNow);
    public void LogError(string message) => _logger.LogError(message, DateTime.UtcNow);
    public void LogCritical(string message) => _logger.LogCritical(message, DateTime.UtcNow);

    private string Format(string message)
    {
        var userId = _userContext.UserId.ToString();

        return $"[UserId: {userId}] {message}";
    }
}
