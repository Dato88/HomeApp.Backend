namespace HomeApp.Bff.Services;

public static class SessionKeys
{
    public const string PkceVerifier = "pkce_verifier";
    public const string OAuthState = "oauth_state";
    public const string AccessToken = "access_token";
    public const string RefreshToken = "refresh_token";
    public const string IdToken = "id_token";
    public const string AccessTokenExpiresAtUtc = "access_token_expires_at_utc";
}
