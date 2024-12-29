namespace app.business.Services;
public interface IFileManager : IFileUploader, IFileDownloader
{
    Task<bool> DeleteAsync(string fileName);
}
