
using app.business.Services;

namespace app.infrastructure.Services;
public class FileManager(IFileDownloader fileDownloader, IFileUploader fileUploader) : IFileManager
{
    private readonly IFileDownloader _fileDownloader = fileDownloader;
    private readonly IFileUploader _fileUploader = fileUploader;

    public Task<bool> DeleteAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> DownloadAsync(string fileId, CancellationToken cancellationToken = default)
    {
        return _fileDownloader.DownloadAsync(fileId, cancellationToken);
    }

    public Task<string> UploadAsync(Stream stream, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        return _fileUploader.UploadAsync(stream, fileName, relativeLocation, cancellationToken);
    }

    public Task<string> UploadAsync(byte[] data, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        return _fileUploader.UploadAsync(data, fileName, relativeLocation, cancellationToken);
    }
}
