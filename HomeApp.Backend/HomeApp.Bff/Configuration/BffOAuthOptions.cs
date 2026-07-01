namespace HomeApp.Bff.Configuration;

public sealed class BffOAuthOptions
{
    public const string SectionName = "OAuth";

    public string Authority { get; set; } = default!;

    public string ClientId { get; set; } = default!;

    public string ClientSecret { get; set; } = default!;

    public string RedirectUri { get; set; } = "http://localhost:4200/auth/callback";

    public string PostLogoutRedirectUri { get; set; } = "http://localhost:4200";

    public string PostLoginRedirectUri { get; set; } = "http://localhost:4200";

    public string AngularOrigin { get; set; } = "http://localhost:4200";
}
