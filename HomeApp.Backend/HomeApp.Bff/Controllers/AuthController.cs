using HomeApp.Bff.Configuration;
using HomeApp.Bff.Services;
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

        SetTokenCookies(tokens);

        HttpContext.Session.Remove(SessionKeys.PkceVerifier);
        HttpContext.Session.Remove(SessionKeys.OAuthState);

        logger.LogInformation("User authenticated via BFF");

        return Redirect(_options.PostLogoutRedirectUri);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Request.Cookies.TryGetValue(TokenCookieNames.IdToken, out var idToken);

        ClearTokenCookies();

        var logoutUrl = oauthService.BuildLogoutUrl(idToken);
        return Redirect(logoutUrl);
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        var authenticated = HttpContext.Request.Cookies.ContainsKey(TokenCookieNames.AccessToken);
        return Ok(new { authenticated });
    }

    private void SetTokenCookies(TokenResponse tokens)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Path = "/"
        };

        Response.Cookies.Append(TokenCookieNames.AccessToken, tokens.AccessToken, cookieOptions);

        if (!string.IsNullOrWhiteSpace(tokens.RefreshToken))
        {
            Response.Cookies.Append(TokenCookieNames.RefreshToken, tokens.RefreshToken, cookieOptions);
        }

        if (!string.IsNullOrWhiteSpace(tokens.IdToken))
        {
            Response.Cookies.Append(TokenCookieNames.IdToken, tokens.IdToken, cookieOptions);
        }
    }

    private void ClearTokenCookies()
    {
        Response.Cookies.Delete(TokenCookieNames.AccessToken);
        Response.Cookies.Delete(TokenCookieNames.RefreshToken);
        Response.Cookies.Delete(TokenCookieNames.IdToken);
    }
}
