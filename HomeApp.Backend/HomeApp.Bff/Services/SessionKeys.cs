namespace HomeApp.Bff.Services;

public static class SessionKeys
{
    public const string PkceVerifier = "pkce_verifier";
    public const string OAuthState = "oauth_state";
}

public static class TokenCookieNames
{
    public const string AccessToken = "access_token";
    public const string RefreshToken = "refresh_token";
    public const string IdToken = "id_token";
}
