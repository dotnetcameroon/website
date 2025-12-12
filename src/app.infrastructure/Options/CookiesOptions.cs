namespace app.infrastructure.Options;

public sealed class CookiesOptions
{
    public const string SectionName = "CookiesOptions";
    public string Issuer { get; init; } = string.Empty;
    public string LoginPath { get; init; } = string.Empty;
    public string LogoutPath { get; init; } = string.Empty;
    public string AccessDeniedPath { get; init; } = string.Empty;
    public int ExpirationInDays { get; init; }
}
