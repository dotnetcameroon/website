using app.Api.Admin.Dtos;
using app.business.Services;
using app.domain.Models.EventAggregate.Entities;

namespace app.Api.Admin;

public static class AdminPartnersApi
{
    public static RouteGroupBuilder MapAdminPartnersApi(this RouteGroupBuilder group)
    {
        group.MapGet("/partners", async (
            IPartnerService partnerService,
            CancellationToken ct) =>
        {
            var result = await partnerService.GetAllAsync(ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.Ok(result.Value.Select(p => new PartnerResponse(p.Id, p.Name, p.Logo, p.Website)));
        });

        group.MapGet("/partners/{id:guid}", async (
            Guid id,
            IPartnerService partnerService,
            CancellationToken ct) =>
        {
            var result = await partnerService.GetAsync(id, ct);
            if (result.IsError)
                return Results.NotFound(result.FirstError.Description);

            var p = result.Value;
            return Results.Ok(new PartnerResponse(p.Id, p.Name, p.Logo, p.Website));
        });

        group.MapPost("/partners", async (
            CreateOrUpdatePartnerRequest request,
            IPartnerService partnerService,
            CancellationToken ct) =>
        {
            var partner = Partner.Create(request.Name, request.Logo, request.Website);
            var result = await partnerService.CreateAsync(partner, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            var p = result.Value;
            return Results.Created($"/api/admin/partners/{p.Id}", new PartnerResponse(p.Id, p.Name, p.Logo, p.Website));
        });

        group.MapPut("/partners/{id:guid}", async (
            Guid id,
            CreateOrUpdatePartnerRequest request,
            IPartnerService partnerService,
            CancellationToken ct) =>
        {
            var getResult = await partnerService.GetAsync(id, ct);
            if (getResult.IsError)
                return Results.NotFound(getResult.FirstError.Description);

            var partner = Partner.Create(request.Name, request.Logo, request.Website);
            var result = await partnerService.UpdateAsync(partner, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            var p = result.Value;
            return Results.Ok(new PartnerResponse(p.Id, p.Name, p.Logo, p.Website));
        });

        group.MapDelete("/partners/{id:guid}", async (
            Guid id,
            IPartnerService partnerService,
            CancellationToken ct) =>
        {
            var result = await partnerService.DeleteAsync(id, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.NoContent();
        });

        return group;
    }
}
