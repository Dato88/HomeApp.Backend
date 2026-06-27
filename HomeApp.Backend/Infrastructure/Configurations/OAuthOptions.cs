namespace Infrastructure.Configurations;

public sealed class OAuthOptions
{
    public const string SectionName = "OAuth";

    public string Authority { get; set; } = default!;

    public string[] ValidAudiences { get; set; } = [];
}
