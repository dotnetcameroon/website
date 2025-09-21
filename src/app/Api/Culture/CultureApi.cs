using Microsoft.AspNetCore.Localization;

namespace app.Api.Culture;

public static class CultureApi
{
    public static IEndpointRouteBuilder MapCultureApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/setculture", (HttpContext http, string culture, string? returnUrl) =>
        {
            if (string.IsNullOrEmpty(culture)) culture = "en-US";
            var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Lax,
                Secure = http.Request.IsHttps
            };
            http.Response.Cookies.Append("AppCulture", cookieValue, cookieOptions);

            return Results.Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
        });

        return endpoints;
    }
}
