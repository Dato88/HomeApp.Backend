using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using HomeApp.Bff.Configuration;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace HomeApp.Bff.Services;

public sealed class OAuthService(
    IHttpClientFactory httpClientFactory,
    IOptions<BffOAuthOptions> options,
    ILogger<OAuthService> logger)
{
    private readonly BffOAuthOptions _options = options.Value;

    public string BuildAuthorizationUrl(string state, string codeChallenge)
    {
        var query = new Dictionary<string, string?>
        {
            ["client_id"] = _options.ClientId,
            ["response_type"] = "code",
            ["scope"] = "openid profile email offline_access",
            ["redirect_uri"] = _options.RedirectUri,
            ["state"] = state,
            ["code_challenge"] = codeChallenge,
            ["code_challenge_method"] = "S256"
        };

        return QueryHelpers.AddQueryString(
            $"{_options.Authority.TrimEnd('/')}/protocol/openid-connect/auth",
            query!);
    }

    public async Task<TokenResponse?> ExchangeCodeForTokenAsync(
        string code,
        string codeVerifier,
        CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("keycloak");

        using var request = new HttpRequestMessage(HttpMethod.Post,
            $"{_options.Authority.TrimEnd('/')}/protocol/openid-connect/token");

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["code"] = code,
            ["redirect_uri"] = _options.RedirectUri,
            ["code_verifier"] = codeVerifier
        });

        var response = await client.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Token exchange failed with status {StatusCode}", response.StatusCode);
            return null;
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<TokenResponse>(stream, cancellationToken: cancellationToken);
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("keycloak");

        using var request = new HttpRequestMessage(HttpMethod.Post,
            $"{_options.Authority.TrimEnd('/')}/protocol/openid-connect/token");

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["client_id"] = _options.ClientId,
            ["client_secret"] = _options.ClientSecret,
            ["refresh_token"] = refreshToken
        });

        var response = await client.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogWarning("Token refresh failed with status {StatusCode}", response.StatusCode);
            return null;
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return await JsonSerializer.DeserializeAsync<TokenResponse>(stream, cancellationToken: cancellationToken);
    }

    public string BuildLogoutUrl(string? idTokenHint = null)
    {
        var query = new Dictionary<string, string?>
        {
            ["client_id"] = _options.ClientId,
            ["post_logout_redirect_uri"] = _options.PostLogoutRedirectUri
        };

        if (!string.IsNullOrWhiteSpace(idTokenHint))
            query["id_token_hint"] = idTokenHint;

        return QueryHelpers.AddQueryString(
            $"{_options.Authority.TrimEnd('/')}/protocol/openid-connect/logout",
            query!);
    }
}

public sealed class TokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = default!;

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("id_token")]
    public string? IdToken { get; set; }
}
