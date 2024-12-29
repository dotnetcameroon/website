namespace app.Options;

public sealed class AzureBlobOptions
{
    public static string SectionName => "AzureBlobOptions";
    public string ConnectionString { get; init; } = string.Empty;
}
