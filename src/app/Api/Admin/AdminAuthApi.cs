using System.Security.Claims;
using app.Api.Admin.Dtos;
using app.shared.Utilities;

namespace app.Api.Admin;

public static class AdminAuthApi
{
    public static RouteGroupBuilder MapAdminAuthApi(this RouteGroupBuilder group)
    {
        group.MapGet("/auth/me", (ClaimsPrincipal user) =>
        {
            var email = user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var role = user.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            return Results.Ok(new AuthMeResponse(email, role));
        });

        return group;
    }
}
