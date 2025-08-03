using app.domain.Models.Common;
using app.domain.Models.EventAggregate.ValueObjects;
using Host = app.domain.Models.EventAggregate.ValueObjects.Host;

namespace app.domain.Models.EventAggregate.Entities;
public sealed class Activity : Entity<Guid>
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Host Host { get; private set; }
    public ActivitySchedule Schedule { get; private set; }
    public Guid EventId { get; private set; }
    public Event Event { get; private set; }

    private Activity(Guid id, string title, string description, Host host, ActivitySchedule schedule, Event @event) : base(id)
    {
        Title = title;
        Description = description;
        Host = host;
        Schedule = schedule;
        Event = @event;
        EventId = @event.Id;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    private Activity()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    {
    }

    public static Activity Create(
        string title,
        string description,
        Host host,
        ActivitySchedule schedule,
        Event @event)
    {
        return new Activity(Guid.NewGuid(), title, description, host, schedule, @event);
    }
}
