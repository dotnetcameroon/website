using app.business.Services;
using app.infrastructure.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace app.infrastructure.Services;

public class FileUploader(IMinioClient minioClient, IOptions<MinioOptions> options) : IFileUploader
{
    private readonly MinioOptions _options = options.Value;

    public async Task<string> UploadAsync(Stream stream, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        await EnsureBucketExistsAsync(cancellationToken);

        var objectName = string.IsNullOrWhiteSpace(relativeLocation)
            ? fileName
            : $"{relativeLocation.TrimEnd('/')}/{fileName}";

        var contentType = GetContentType(fileName);

        await minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length)
            .WithContentType(contentType),
            cancellationToken);

        return BuildPublicUrl(objectName);
    }

    public async Task<string> UploadAsync(byte[] data, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(data);
        return await UploadAsync(stream, fileName, relativeLocation, cancellationToken);
    }

    private async Task EnsureBucketExistsAsync(CancellationToken ct)
    {
        var exists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_options.BucketName), ct);

        if (!exists)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_options.BucketName), ct);

            // Set public read policy so images are accessible via URL
            var policy = $$"""
            {
                "Version": "2012-10-17",
                "Statement": [{
                    "Effect": "Allow",
                    "Principal": {"AWS": ["*"]},
                    "Action": ["s3:GetObject"],
                    "Resource": ["arn:aws:s3:::{{_options.BucketName}}/*"]
                }]
            }
            """;

            await minioClient.SetPolicyAsync(
                new SetPolicyArgs().WithBucket(_options.BucketName).WithPolicy(policy), ct);
        }
    }

    private string BuildPublicUrl(string objectName)
    {
        if (!string.IsNullOrWhiteSpace(_options.PublicUrl))
            return $"{_options.PublicUrl.TrimEnd('/')}/{_options.BucketName}/{objectName}";

        var endpoint = _options.PublicEndpoint.StartsWith("http://") || _options.PublicEndpoint.StartsWith("https://")
            ? _options.PublicEndpoint
            : $"{(_options.UseSsl ? "https" : "http")}://{_options.PublicEndpoint}";

        return $"{endpoint.TrimEnd('/')}/{_options.BucketName}/{objectName}";
    }

    private static string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream",
        };
    }
}
