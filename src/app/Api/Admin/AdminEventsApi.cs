using app.Api.Admin.Dtos;
using app.business.Services;
using app.domain.Models.EventAggregate.ValueObjects;
using app.domain.ViewModels;

namespace app.Api.Admin;

public static class AdminEventsApi
{
    public static RouteGroupBuilder MapAdminEventsApi(this RouteGroupBuilder group)
    {
        group.MapGet("/events", async (
            int? page,
            int? size,
            string? search,
            IEventService eventService,
            CancellationToken ct) =>
        {
            var p = page ?? 1;
            var s = size ?? 10;

            var result = string.IsNullOrWhiteSpace(search)
                ? await eventService.GetAllAsync(p, s, ct)
                : await eventService.SearchAsync(e => e.Title.Contains(search), p, s, ct);

            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            var pagedList = result.Value;
            var response = new PagedResponse<EventSummaryResponse>(
                pagedList.Items.Select(e => new EventSummaryResponse(
                    e.Id,
                    e.Title,
                    new EventScheduleDto(e.Schedule.Start, e.Schedule.End, e.Schedule.IsAllDay),
                    e.Status,
                    e.Type,
                    e.HostingModel)).ToList(),
                pagedList.TotalCount,
                pagedList.PageNumber,
                pagedList.PageSize,
                pagedList.TotalPages,
                pagedList.HasPreviousPage,
                pagedList.HasNextPage);

            return Results.Ok(response);
        });

        group.MapGet("/events/{id:guid}", async (
            Guid id,
            IEventService eventService,
            CancellationToken ct) =>
        {
            var result = await eventService.GetAsync(id, ct);
            if (result.IsError)
                return Results.NotFound(result.FirstError.Description);

            var e = result.Value;
            return Results.Ok(MapToDetailResponse(e));
        });

        group.MapPost("/events", async (
            CreateOrUpdateEventRequest request,
            IEventService eventService,
            IPartnerService partnerService,
            CancellationToken ct) =>
        {
            var eventModel = await MapToEventModel(request, partnerService, ct);
            var result = await eventService.CreateAsync(eventModel, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.Created($"/api/admin/events/{result.Value.Id}", MapToDetailResponse(result.Value));
        });

        group.MapPut("/events/{id:guid}", async (
            Guid id,
            CreateOrUpdateEventRequest request,
            IEventService eventService,
            IPartnerService partnerService,
            CancellationToken ct) =>
        {
            var eventModel = await MapToEventModel(request, partnerService, ct);
            var result = await eventService.UpdateAsync(id, eventModel, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.Ok(MapToDetailResponse(result.Value));
        });

        group.MapDelete("/events/{id:guid}", async (
            Guid id,
            IEventService eventService,
            CancellationToken ct) =>
        {
            var result = await eventService.DeleteEvent(id, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.NoContent();
        });

        group.MapPost("/events/{id:guid}/publish", async (
            Guid id,
            IEventService eventService,
            CancellationToken ct) =>
        {
            var result = await eventService.PublishAsync(id, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.Ok(MapToDetailResponse(result.Value));
        });

        group.MapPost("/events/{id:guid}/cancel", async (
            Guid id,
            IEventService eventService,
            CancellationToken ct) =>
        {
            var result = await eventService.CancelAsync(id, ct);
            if (result.IsError)
                return Results.Problem(result.FirstError.Description);

            return Results.Ok(MapToDetailResponse(result.Value));
        });

        group.MapPost("/events/{id:guid}/mark-passed", async (
            Guid id,
            IEventService eventService,
            CancellationToken ct) =>
        {
            var getResult = await eventService.GetAsync(id, ct);
            if (getResult.IsError)
                return Results.NotFound(getResult.FirstError.Description);

            await eventService.MarkPassedEventsAsync();
            return Results.NoContent();
        });

        return group;
    }

    private static EventDetailResponse MapToDetailResponse(domain.Models.EventAggregate.Event e)
    {
        return new EventDetailResponse(
            e.Id,
            e.Title,
            e.Description,
            new EventScheduleDto(e.Schedule.Start, e.Schedule.End, e.Schedule.IsAllDay),
            e.Type,
            e.Status,
            e.HostingModel,
            e.Attendance,
            e.RegistrationLink,
            e.ImageUrl,
            e.Images,
            e.Location,
            e.Activities.Select(a => new ActivityResponse(
                a.Id,
                a.Title,
                a.Description,
                new HostDto(a.Host.Name, a.Host.Email, a.Host.ImageUrl),
                new ActivityScheduleDto(a.Schedule.Start, a.Schedule.End))).ToList(),
            e.Partners.Select(p => new PartnerResponse(p.Id, p.Name, p.Logo, p.Website)).ToList());
    }

    private static async Task<EventModel> MapToEventModel(
        CreateOrUpdateEventRequest request,
        IPartnerService partnerService,
        CancellationToken ct)
    {
        var partners = new List<domain.Models.EventAggregate.Entities.Partner>();
        foreach (var partnerId in request.PartnerIds)
        {
            var partnerResult = await partnerService.GetAsync(partnerId, ct);
            if (!partnerResult.IsError)
                partners.Add(partnerResult.Value);
        }

        return new EventModel
        {
            Title = request.Title,
            Description = request.Description,
            Schedule = EventSchedule.Create(request.Schedule.Start, request.Schedule.End, request.Schedule.IsAllDay),
            Type = request.Type,
            Status = request.Status,
            HostingModel = request.HostingModel,
            Attendance = request.Attendance,
            RegistrationLink = request.RegistrationLink,
            ImageUrl = request.ImageUrl,
            Images = request.Images,
            Location = request.Location,
            Partners = partners,
            Activities = request.Activities.Select(a => new ActivityModel
            {
                Title = a.Title,
                Description = a.Description,
                Host = new HostModel
                {
                    Name = a.Host.Name,
                    Email = a.Host.Email,
                    ImageUrl = a.Host.ImageUrl
                },
                Schedule = ActivitySchedule.Create(a.Schedule.Start, a.Schedule.End)
            }).ToList()
        };
    }
}
