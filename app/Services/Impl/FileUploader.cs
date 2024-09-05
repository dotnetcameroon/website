namespace app.Services.Impl;
internal class FileUploader(IWebHostEnvironment webEnvironment) : IFileUploader
{
    private readonly IWebHostEnvironment _webEnvironment = webEnvironment;
    private string UploadPath => Path.Combine(_webEnvironment.ContentRootPath, "files");

    public Task<string> UploadAsync(Stream stream, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> UploadAsync(byte[] data, string fileName, string relativeLocation, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
