using app.Api.Admin.Dtos;
using app.business.Services;
using app.domain.Models.BannerAggregate;
using app.domain.ViewModels;

namespace app.Api.Admin;

public static class AdminBannersApi
{
    public static RouteGroupBuilder MapAdminBannersApi(this RouteGroupBuilder group)
    {
        group.MapGet("/banners", async (
            int? page,
            int? size,
            IBannerService bannerService,
            CancellationToken ct) =>
        {
            var p = page ?? 1;
            var s = size ?? 10;

            var result = await bannerService.GetAllAsync(p, s, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            var pagedList = result.Value;
            var response = new PagedResponse<BannerResponse>(
                pagedList.Items.Select(MapToResponse).ToList(),
                pagedList.TotalCount,
                pagedList.PageNumber,
                pagedList.PageSize,
                pagedList.TotalPages,
                pagedList.HasPreviousPage,
                pagedList.HasNextPage);

            return Results.Ok(response);
        });

        group.MapGet("/banners/{id:guid}", async (
            Guid id,
            IBannerService bannerService,
            CancellationToken ct) =>
        {
            var result = await bannerService.GetAsync(id, ct);
            if (result.IsError)
                return Results.NotFound(result.FirstError.Description);

            return Results.Ok(MapToResponse(result.Value));
        });

        group.MapPost("/banners", async (
            CreateOrUpdateBannerRequest request,
            IBannerService bannerService,
            CancellationToken ct) =>
        {
            var result = await bannerService.CreateAsync(MapToModel(request), ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.Created($"/api/admin/banners/{result.Value.Id}", MapToResponse(result.Value));
        });

        group.MapPut("/banners/{id:guid}", async (
            Guid id,
            CreateOrUpdateBannerRequest request,
            IBannerService bannerService,
            CancellationToken ct) =>
        {
            var result = await bannerService.UpdateAsync(id, MapToModel(request), ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.Ok(MapToResponse(result.Value));
        });

        group.MapDelete("/banners/{id:guid}", async (
            Guid id,
            IBannerService bannerService,
            CancellationToken ct) =>
        {
            var result = await bannerService.DeleteAsync(id, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.NoContent();
        });

        return group;
    }

    private static BannerResponse MapToResponse(Banner b) => new(
        b.Id,
        b.TitleEn,
        b.TitleFr,
        b.SubtitleEn,
        b.SubtitleFr,
        b.MessageEn,
        b.MessageFr,
        b.Variant,
        b.StartDate,
        b.EndDate,
        b.Link,
        b.LinkLabelEn,
        b.LinkLabelFr,
        b.Dismissible,
        b.Priority,
        b.IsEnabled,
        b.CreatedAt);

    private static BannerModel MapToModel(CreateOrUpdateBannerRequest r) => new()
    {
        TitleEn = r.TitleEn,
        TitleFr = r.TitleFr,
        SubtitleEn = r.SubtitleEn,
        SubtitleFr = r.SubtitleFr,
        MessageEn = r.MessageEn,
        MessageFr = r.MessageFr,
        Variant = r.Variant,
        StartDate = r.StartDate,
        EndDate = r.EndDate,
        Link = r.Link,
        LinkLabelEn = r.LinkLabelEn,
        LinkLabelFr = r.LinkLabelFr,
        Dismissible = r.Dismissible,
        Priority = r.Priority,
        IsEnabled = r.IsEnabled,
    };
}
