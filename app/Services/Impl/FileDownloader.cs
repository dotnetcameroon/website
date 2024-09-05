

namespace app.Services.Impl;
internal class FileDownloader : IFileDownloader
{
    public Task<Stream> DownloadAsync(string fileId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
