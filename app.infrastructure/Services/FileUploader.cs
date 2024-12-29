using app.business.Services;

namespace app.infrastructure.Services;
public class FileUploader : IFileUploader
{
    public Task<string> UploadAsync(Stream stream, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> UploadAsync(byte[] data, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
