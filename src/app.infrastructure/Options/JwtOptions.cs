namespace app.infrastructure.Options;

public sealed class JwtOptions
{
    public const string SectionName = "JwtOptions";
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string Secret { get; init; } = string.Empty;
    public int ExpirationInMinutes { get; init; }
}
