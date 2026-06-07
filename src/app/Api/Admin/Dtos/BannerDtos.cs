using app.domain.Models.BannerAggregate.Enums;

namespace app.Api.Admin.Dtos;

public record BannerResponse(
    Guid Id,
    string? TitleEn,
    string? TitleFr,
    string? SubtitleEn,
    string? SubtitleFr,
    string MessageEn,
    string MessageFr,
    BannerVariant Variant,
    DateTime StartDate,
    DateTime EndDate,
    string? Link,
    string? LinkLabelEn,
    string? LinkLabelFr,
    bool Dismissible,
    int Priority,
    bool IsEnabled,
    DateTime CreatedAt);

public record CreateOrUpdateBannerRequest(
    string? TitleEn,
    string? TitleFr,
    string? SubtitleEn,
    string? SubtitleFr,
    string MessageEn,
    string MessageFr,
    BannerVariant Variant,
    DateTime StartDate,
    DateTime EndDate,
    string? Link,
    string? LinkLabelEn,
    string? LinkLabelFr,
    bool Dismissible,
    int Priority,
    bool IsEnabled);
