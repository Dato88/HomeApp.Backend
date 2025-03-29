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

    public void LogTrace(string message) => _logger.LogTrace(Format(message), DateTime.UtcNow);
    public void LogDebug(string message) => _logger.LogDebug(Format(message), DateTime.UtcNow);
    public void LogInformation(string message) => _logger.LogInformation(Format(message), DateTime.UtcNow);
    public void LogWarning(string message) => _logger.LogWarning(Format(message), DateTime.UtcNow);
    public void LogError(string message) => _logger.LogError(Format(message), DateTime.UtcNow);
    public void LogCritical(string message) => _logger.LogCritical(Format(message), DateTime.UtcNow);

    private string Format(string message)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true
            ? _userContext.UserId.ToString()
            : "Unauthenticated";

        return $"[UserId: {userId}] {message}";
    }
}
