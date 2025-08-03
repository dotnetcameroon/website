namespace app.business.Services;
public interface IFileDownloader
{
    Task<Stream> DownloadAsync(string fileId, CancellationToken cancellationToken = default);
}
