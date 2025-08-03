

using app.business.Services;

namespace app.infrastructure.Services;
public class FileDownloader : IFileDownloader
{
    public Task<Stream> DownloadAsync(string fileId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
