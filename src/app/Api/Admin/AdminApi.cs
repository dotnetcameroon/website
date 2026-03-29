using app.shared.Utilities;

namespace app.Api.Admin;

public static class AdminApi
{
    public static IEndpointRouteBuilder MapAdminApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/admin")
            .RequireAuthorization(Policies.AdminOnly)
            .DisableAntiforgery();

        group.MapAdminAuthApi();
        group.MapAdminEventsApi();
        group.MapAdminPartnersApi();
        group.MapAdminAppsApi();

        return endpoints;
    }
}
