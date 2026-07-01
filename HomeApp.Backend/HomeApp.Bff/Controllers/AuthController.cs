using HomeApp.Bff.Configuration;
using HomeApp.Bff.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace HomeApp.Bff.Controllers;

[ApiController]
[Route("auth")]
[EnableRateLimiting("auth")]
public sealed class AuthController(
    OAuthService oauthService,
    PkceService pkceService,
    TokenSessionService tokenSessionService,
    IOptions<BffOAuthOptions> options,
    ILogger<AuthController> logger) : ControllerBase
{
    private readonly BffOAuthOptions _options = options.Value;

    [HttpGet("login")]
    public IActionResult Login()
    {
        var (verifier, challenge) = pkceService.GenerateChallenge();
        var state = pkceService.GenerateState();

        HttpContext.Session.SetString(SessionKeys.PkceVerifier, verifier);
        HttpContext.Session.SetString(SessionKeys.OAuthState, state);

        var authorizationUrl = oauthService.BuildAuthorizationUrl(state, challenge);
        return Redirect(authorizationUrl);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
        [FromQuery] string? code,
        [FromQuery] string? state,
        CancellationToken cancellationToken)
    {
        var expectedState = HttpContext.Session.GetString(SessionKeys.OAuthState);
        var codeVerifier = HttpContext.Session.GetString(SessionKeys.PkceVerifier);

        if (string.IsNullOrWhiteSpace(code)
            || string.IsNullOrWhiteSpace(state)
            || state != expectedState
            || string.IsNullOrWhiteSpace(codeVerifier))
        {
            logger.LogWarning("OAuth callback validation failed");
            return BadRequest("Invalid OAuth callback.");
        }

        var tokens = await oauthService.ExchangeCodeForTokenAsync(code, codeVerifier, cancellationToken);

        if (tokens is null)
            return BadRequest("Token exchange failed.");

        tokenSessionService.StoreTokens(tokens);

        HttpContext.Session.Remove(SessionKeys.PkceVerifier);
        HttpContext.Session.Remove(SessionKeys.OAuthState);

        logger.LogInformation("User authenticated via BFF");

        return Redirect(_options.PostLoginRedirectUri);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        var idToken = tokenSessionService.GetIdToken();

        tokenSessionService.ClearTokens();
        HttpContext.Session.Clear();

        var logoutUrl = oauthService.BuildLogoutUrl(idToken);
        return Redirect(logoutUrl);
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        var authenticated = tokenSessionService.IsAuthenticated();
        return Ok(new { authenticated });
    }

    [HttpGet("antiforgery")]
    public IActionResult Antiforgery([FromServices] IAntiforgery antiforgery)
    {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);
        return Ok(new { token = tokens.RequestToken });
    }
}
