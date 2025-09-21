using app.business.Services;

namespace app.Api.DebugMode;

public static class DebugModeApi
{
    public static IEndpointRouteBuilder MapDebugModeApi(this IEndpointRouteBuilder endpoints)
    {
        // Map dev mode endpoints
        endpoints.MapGet("/dev-mode/toggle", (HttpContext http, IDevModeService devModeService) =>
        {
            if (devModeService.IsDevMode)
            {
                devModeService.DisableDevMode();
            }
            else
            {
                devModeService.EnableDevMode();
            }

            var returnUrl = http.Request.Headers.Referer.FirstOrDefault() ?? "/";
            return Results.Redirect(returnUrl);
        });

        endpoints.MapGet("/dev-mode/enable", (HttpContext http, IDevModeService devModeService) =>
        {
            devModeService.EnableDevMode();
            var returnUrl = http.Request.Query["returnUrl"].FirstOrDefault() ?? "/";
            return Results.Redirect(returnUrl);
        });

        endpoints.MapGet("/dev-mode/disable", (HttpContext http, IDevModeService devModeService) =>
        {
            devModeService.DisableDevMode();
            var returnUrl = http.Request.Query["returnUrl"].FirstOrDefault() ?? "/";
            return Results.Redirect(returnUrl);
        });

        endpoints.MapGet("/dev-mode/status", (IDevModeService devModeService) =>
        {
            return Results.Json(new { isDevMode = devModeService.IsDevMode });
        });

        return endpoints;
    }
}
