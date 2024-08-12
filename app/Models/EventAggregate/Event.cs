using app.Models.Common;
using app.Models.EventAggregate.Entities;
using app.Models.EventAggregate.Enums;
using app.Models.EventAggregate.ValueObjects;
using Domain.Common.Utils;

namespace app.Models.EventAggregate;
public sealed class Event : Entity<Guid>, IAggregateRoot
{
    private readonly List<Partner> _partners = [];
    private readonly List<Activity> _activities = [];
    private readonly List<IDomainEvent> _domainEvents = [];
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Schedule Schedule { get; private set; }
    public EventType Type { get; private set; }
    public EventStatus Status { get; private set; }
    public EventHostingModel HostingModel { get; private set; }
    public int? Attendance { get; private set; }
    public string? RegistrationLink { get; private set; }
    /// <summary>
    /// The image url of the event
    /// </summary>
    public string? ImageUrl { get; private set; }

    /// <summary>
    /// The list of images links separated by comma
    /// </summary>
    public string? Images { get; private set; }
    public IReadOnlyList<Partner> Partners => [.. _partners];
    public IReadOnlyList<Activity> Activities => [.. _activities];
    public IReadOnlyList<IDomainEvent> DomainEvents => [.. _domainEvents];

    // CONSTRUCTOR
    private Event(
        Guid id,
        string title,
        string description,
        Schedule schedule,
        EventType type,
        EventStatus status,
        EventHostingModel hostingModel,
        string? imageUrl,
        string? images,
        int? attendance,
        string? registrationLink) : base(id)
    {
        Title = title;
        Description = description;
        Schedule = schedule;
        Type = type;
        Status = status;
        HostingModel = hostingModel;
        Attendance = attendance;
        Images = images;
        ImageUrl = imageUrl;
        RegistrationLink = registrationLink;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public Event()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    public void AddActivity(Activity activity)
    {
        _activities.Add(activity);
    }

    public void RemoveActivity(Activity activity)
    {
        _activities.Remove(activity);
    }

    public void AddPartner(Partner partner)
    {
        _partners.Add(partner);
    }

    public void RemovePartner(Partner partner)
    {
        _partners.Remove(partner);
    }

    public void Publish()
    {
        Status = EventStatus.CommingSoon;
    }

    public void Cancel()
    {
        Status = EventStatus.Cancelled;
    }

    public void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public static Event Create(
        string title,
        string description,
        Schedule schedule,
        EventType type,
        EventStatus status,
        EventHostingModel hostingModel,
        string? imageUrl,
        string? images,
        int? attendance,
        string? registrationLink,
        List<Activity>? activities,
        List<Partner>? partners)
    {
        var @event = new Event(
            Guid.NewGuid(),
            title,
            description,
            schedule,
            type,
            status,
            hostingModel,
            imageUrl,
            images,
            attendance,
            registrationLink);

        if(activities is not null)
        {
            foreach (var activity in activities)
            {
                @event.AddActivity(activity);
            }
        }

        if(partners is not null)
        {
            foreach (var partner in partners)
            {
                @event.AddPartner(partner);
            }
        }

        return @event;
    }
}
