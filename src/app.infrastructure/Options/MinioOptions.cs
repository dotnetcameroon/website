namespace app.infrastructure.Options;

public sealed class MinioOptions
{
    public const string SectionName = "Minio";
    public string Endpoint { get; init; } = "localhost:9000";
    public string PublicEndpoint { get; init; } = "localhost:9000";
    public string AccessKey { get; init; } = "minioadmin";
    public string SecretKey { get; init; } = "minioadmin";
    public string BucketName { get; init; } = "uploads";
    public bool UseSsl { get; init; } = false;
    public string PublicUrl { get; init; } = "";
}
