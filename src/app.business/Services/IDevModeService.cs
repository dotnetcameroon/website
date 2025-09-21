namespace app.business.Services;

public interface IDevModeService
{
    bool IsDevMode { get; }
    void EnableDevMode();
    void DisableDevMode();
    string GetDevModeCookieName();
}