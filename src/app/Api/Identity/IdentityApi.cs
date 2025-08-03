using app.business.Services;
using Microsoft.AspNetCore.Mvc;

namespace app.Api.Identity;
public static class IdentityApi
{
    public static IEndpointRouteBuilder MapIdentityApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/login/external", async (
            [FromBody] LoginExternalRequest request, IIdentityService service) =>
        {
            var result = await service.LoginAppAsync(request.ApplicationId, request.ApplicationSecret);
            return result.Match(
                token => Results.Ok(token),
                error => Problem(error)
            );
        });
        return endpoints;
    }
}

public record LoginExternalRequest(Guid ApplicationId, string ApplicationSecret);
