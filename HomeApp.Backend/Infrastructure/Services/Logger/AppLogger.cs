using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Logger;

internal class AppLogger<T> : IAppLogger<T>
{
    private readonly ILogger<T> _logger;
    private readonly IExecutionContextAccessor _executionContext;

    public AppLogger(ILogger<T> logger, IExecutionContextAccessor executionContext)
    {
        _logger = logger;
        _executionContext = executionContext;
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
            var personId = _executionContext.PersonId.ToString();

            return $"[PersonId: {personId}] {message}";
        }
        catch (Exception ex)
        {
            return message;
        }
    }
}
