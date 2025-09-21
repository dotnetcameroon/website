using app.business.Services;
using Microsoft.AspNetCore.Http;

namespace app.infrastructure.Services;

public class DevModeService : IDevModeService
{
    private const string DevModeCookieName = "DebugMode";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DevModeService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsDevMode
    {
        get
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return false;

            return context.Request.Cookies.TryGetValue(DevModeCookieName, out var value)
                && value == "true";
        }
    }

    public void EnableDevMode()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = context.Request.IsHttps,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(30),
            IsEssential = true
        };

        context.Response.Cookies.Append(DevModeCookieName, "true", cookieOptions);
    }

    public void DisableDevMode()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        context.Response.Cookies.Delete(DevModeCookieName);
    }

    public string GetDevModeCookieName()
    {
        return DevModeCookieName;
    }
}