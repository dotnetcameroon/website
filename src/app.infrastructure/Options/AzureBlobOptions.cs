namespace app.infrastructure.Options;

public sealed class AzureBlobOptions
{
    public const string SectionName = "AzureBlobOptions";
    public string ConnectionString { get; init; } = string.Empty;
}
