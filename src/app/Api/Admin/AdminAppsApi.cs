using app.Api.Admin.Dtos;
using app.business.Services;
using app.domain.Models.ExternalAppAggregate;
using Microsoft.AspNetCore.Identity;

namespace app.Api.Admin;

public static class AdminAppsApi
{
    public static RouteGroupBuilder MapAdminAppsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/apps", async (
            int? page,
            int? size,
            IExternalAppService appService,
            CancellationToken ct) =>
        {
            var result = await appService.GetAllAsync(page ?? 1, size ?? 10, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            var pagedList = result.Value;
            var response = new PagedResponse<AppSummaryResponse>(
                pagedList.Items.Select(a => new AppSummaryResponse(a.Id, a.ClientName)).ToList(),
                pagedList.TotalCount,
                pagedList.PageNumber,
                pagedList.PageSize,
                pagedList.TotalPages,
                pagedList.HasPreviousPage,
                pagedList.HasNextPage);

            return Results.Ok(response);
        });

        group.MapPost("/apps", async (
            RegisterAppRequest request,
            IExternalAppService appService,
            IPasswordHasher<Application> passwordHasher,
            CancellationToken ct) =>
        {
            var result = await appService.RegisterAsync(request.Name, passwordHasher, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            var (application, secret) = result.Value;
            return Results.Created(
                $"/api/admin/apps/{application.Id}",
                new RegisterAppResponse(application.Id, application.ClientName, secret));
        });

        return group;
    }
}
