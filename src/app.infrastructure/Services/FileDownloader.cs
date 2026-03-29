using app.business.Services;
using app.infrastructure.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace app.infrastructure.Services;

public class FileDownloader(IMinioClient minioClient, IOptions<MinioOptions> options) : IFileDownloader
{
    private readonly MinioOptions _options = options.Value;

    public async Task<Stream> DownloadAsync(string fileId, CancellationToken cancellationToken = default)
    {
        var memoryStream = new MemoryStream();

        await minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(_options.BucketName)
            .WithObject(fileId)
            .WithCallbackStream(stream => stream.CopyTo(memoryStream)),
            cancellationToken);

        memoryStream.Position = 0;
        return memoryStream;
    }
}
