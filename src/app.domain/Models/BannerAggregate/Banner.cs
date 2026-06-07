using app.domain.Models.BannerAggregate.Enums;
using app.domain.Models.Common;
using app.domain.ViewModels;

namespace app.domain.Models.BannerAggregate;

public sealed class Banner : Entity<Guid>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public string? TitleEn { get; private set; }
    public string? TitleFr { get; private set; }
    public string? SubtitleEn { get; private set; }
    public string? SubtitleFr { get; private set; }
    public string MessageEn { get; private set; } = string.Empty;
    public string MessageFr { get; private set; } = string.Empty;
    public BannerVariant Variant { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string? Link { get; private set; }
    public string? LinkLabelEn { get; private set; }
    public string? LinkLabelFr { get; private set; }
    public bool Dismissible { get; private set; }
    public int Priority { get; private set; }
    public bool IsEnabled { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyList<IDomainEvent> DomainEvents => [.. _domainEvents];

    private Banner(
        Guid id,
        string? titleEn,
        string? titleFr,
        string? subtitleEn,
        string? subtitleFr,
        string messageEn,
        string messageFr,
        BannerVariant variant,
        DateTime startDate,
        DateTime endDate,
        string? link,
        string? linkLabelEn,
        string? linkLabelFr,
        bool dismissible,
        int priority,
        bool isEnabled,
        DateTime createdAt) : base(id)
    {
        TitleEn = titleEn;
        TitleFr = titleFr;
        SubtitleEn = subtitleEn;
        SubtitleFr = subtitleFr;
        MessageEn = messageEn;
        MessageFr = messageFr;
        Variant = variant;
        StartDate = startDate;
        EndDate = endDate;
        Link = link;
        LinkLabelEn = linkLabelEn;
        LinkLabelFr = linkLabelFr;
        Dismissible = dismissible;
        Priority = priority;
        IsEnabled = isEnabled;
        CreatedAt = createdAt;
    }

    public Banner()
    {
    }

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public static Banner Create(BannerModel model)
    {
        return new Banner(
            Guid.NewGuid(),
            model.TitleEn,
            model.TitleFr,
            model.SubtitleEn,
            model.SubtitleFr,
            model.MessageEn,
            model.MessageFr,
            model.Variant,
            DateTime.SpecifyKind(model.StartDate, DateTimeKind.Utc),
            DateTime.SpecifyKind(model.EndDate, DateTimeKind.Utc),
            model.Link,
            model.LinkLabelEn,
            model.LinkLabelFr,
            model.Dismissible,
            model.Priority,
            model.IsEnabled,
            DateTime.UtcNow);
    }

    public void Update(BannerModel model)
    {
        TitleEn = model.TitleEn;
        TitleFr = model.TitleFr;
        SubtitleEn = model.SubtitleEn;
        SubtitleFr = model.SubtitleFr;
        MessageEn = model.MessageEn;
        MessageFr = model.MessageFr;
        Variant = model.Variant;
        StartDate = DateTime.SpecifyKind(model.StartDate, DateTimeKind.Utc);
        EndDate = DateTime.SpecifyKind(model.EndDate, DateTimeKind.Utc);
        Link = model.Link;
        LinkLabelEn = model.LinkLabelEn;
        LinkLabelFr = model.LinkLabelFr;
        Dismissible = model.Dismissible;
        Priority = model.Priority;
        IsEnabled = model.IsEnabled;
    }
}
