using app.domain.Models.EventAggregate.Enums;

namespace app.Api.Admin.Dtos;

public record EventSummaryResponse(
    Guid Id,
    string Title,
    EventScheduleDto Schedule,
    EventStatus Status,
    EventType Type,
    EventHostingModel HostingModel);

public record EventDetailResponse(
    Guid Id,
    string Title,
    string Description,
    EventScheduleDto Schedule,
    EventType Type,
    EventStatus Status,
    EventHostingModel HostingModel,
    int Attendance,
    string RegistrationLink,
    string ImageUrl,
    string Images,
    string? Location,
    List<ActivityResponse> Activities,
    List<PartnerResponse> Partners);

public record EventScheduleDto(DateTime Start, DateTime? End, bool IsAllDay);

public record ActivityResponse(
    Guid Id,
    string Title,
    string Description,
    HostDto Host,
    ActivityScheduleDto Schedule);

public record ActivityScheduleDto(TimeOnly Start, TimeOnly End);

public record HostDto(string Name, string Email, string ImageUrl);

public record CreateOrUpdateEventRequest(
    string Title,
    string Description,
    EventScheduleDto Schedule,
    EventType Type,
    EventStatus Status,
    EventHostingModel HostingModel,
    int Attendance,
    string RegistrationLink,
    string ImageUrl,
    string Images,
    string? Location,
    List<ActivityRequest> Activities,
    List<Guid> PartnerIds);

public record ActivityRequest(
    string Title,
    string Description,
    HostDto Host,
    ActivityScheduleDto Schedule);

public record PagedResponse<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage);
