namespace HomeApp.Bff.Services;

public sealed class TokenSessionService(
    IHttpContextAccessor httpContextAccessor,
    OAuthService oauthService,
    ILogger<TokenSessionService> logger)
{
    private static readonly TimeSpan RefreshBuffer = TimeSpan.FromSeconds(30);

    private ISession Session => httpContextAccessor.HttpContext!.Session;

    public void StoreTokens(TokenResponse tokens)
    {
        Session.SetString(SessionKeys.AccessToken, tokens.AccessToken);

        if (!string.IsNullOrWhiteSpace(tokens.RefreshToken))
            Session.SetString(SessionKeys.RefreshToken, tokens.RefreshToken);
        else
            Session.Remove(SessionKeys.RefreshToken);

        if (!string.IsNullOrWhiteSpace(tokens.IdToken))
            Session.SetString(SessionKeys.IdToken, tokens.IdToken);
        else
            Session.Remove(SessionKeys.IdToken);

        var expiresAt = DateTimeOffset.UtcNow.AddSeconds(tokens.ExpiresIn);
        Session.SetString(SessionKeys.AccessTokenExpiresAtUtc, expiresAt.ToString("O"));
    }

    public string? GetAccessToken() => Session.GetString(SessionKeys.AccessToken);

    public string? GetRefreshToken() => Session.GetString(SessionKeys.RefreshToken);

    public string? GetIdToken() => Session.GetString(SessionKeys.IdToken);

    public bool IsAuthenticated() => !string.IsNullOrWhiteSpace(GetAccessToken());

    public void ClearTokens()
    {
        Session.Remove(SessionKeys.AccessToken);
        Session.Remove(SessionKeys.RefreshToken);
        Session.Remove(SessionKeys.IdToken);
        Session.Remove(SessionKeys.AccessTokenExpiresAtUtc);
    }

    public async Task EnsureValidAccessTokenAsync(CancellationToken cancellationToken)
    {
        var accessToken = GetAccessToken();
        if (string.IsNullOrWhiteSpace(accessToken))
            return;

        var expiresAtValue = Session.GetString(SessionKeys.AccessTokenExpiresAtUtc);
        if (!string.IsNullOrWhiteSpace(expiresAtValue)
            && DateTimeOffset.TryParse(expiresAtValue, out var expiresAt)
            && expiresAt > DateTimeOffset.UtcNow.Add(RefreshBuffer))
            return;

        var refreshToken = GetRefreshToken();
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            logger.LogWarning("Access token expired but no refresh token available");
            ClearTokens();
            return;
        }

        var tokens = await oauthService.RefreshTokenAsync(refreshToken, cancellationToken);
        if (tokens is null)
        {
            logger.LogWarning("Token refresh failed, clearing session tokens");
            ClearTokens();
            return;
        }

        StoreTokens(tokens);
    }
}
