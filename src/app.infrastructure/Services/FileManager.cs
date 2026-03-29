using app.business.Services;
using app.infrastructure.Options;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace app.infrastructure.Services;

public class FileManager(IFileDownloader fileDownloader, IFileUploader fileUploader, IMinioClient minioClient, IOptions<MinioOptions> options) : IFileManager
{
    private readonly MinioOptions _options = options.Value;

    public async Task<bool> DeleteAsync(string fileName)
    {
        try
        {
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_options.BucketName)
                .WithObject(fileName));
            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<Stream> DownloadAsync(string fileId, CancellationToken cancellationToken = default)
    {
        return fileDownloader.DownloadAsync(fileId, cancellationToken);
    }

    public Task<string> UploadAsync(Stream stream, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        return fileUploader.UploadAsync(stream, fileName, relativeLocation, cancellationToken);
    }

    public Task<string> UploadAsync(byte[] data, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        return fileUploader.UploadAsync(data, fileName, relativeLocation, cancellationToken);
    }
}
