using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Logger;

public class AppLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;
    private readonly IUserContext _userContext;

    public AppLogger(ILogger<T> logger, IUserContext userContext, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _userContext = userContext;
    }

    public void LogTrace(string message) => _logger.LogTraceFormatted(Format(message));
    public void LogDebug(string message) => _logger.LogDebugFormatted(Format(message));
    public void LogInformation(string message) => _logger.LogInformationFormatted(Format(message));
    public void LogWarning(string message) => _logger.LogWarningFormatted(Format(message));
    public void LogError(string message) => _logger.LogErrorFormatted(Format(message));
    public void LogCritical(string message) => _logger.LogCriticalFormatted(Format(message));

    private string Format(string message)
    {
        try
        {
            var personId = _userContext.PersonId.ToString();

            return $"[PersonId: {personId}] {message}";
        }
        catch (Exception ex)
        {
            return message;
        }
    }
}
