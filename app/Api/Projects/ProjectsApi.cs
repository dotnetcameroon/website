using app.shared.Utilities;

namespace app.Api.Projects;
public static class ProjectsApi
{
    public static IEndpointRouteBuilder MapProjectsApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/projects", () => "Hello Projects!")
            .RequireAuthorization(Policies.JwtAuthOnly);

        return endpoints;
    }
}
