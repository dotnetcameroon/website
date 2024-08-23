using app.Models.Common;
using app.Models.EventAggregate.ValueObjects;
using Host = app.Models.EventAggregate.ValueObjects.Host;

namespace app.Models.EventAggregate.Entities;
public sealed class Activity : Entity<Guid>
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Host Host { get; private set; }
    public Schedule Schedule { get; private set; }
    public Guid EventId { get; private set; }

    private Activity(Guid id, string title, string description, Host host, Schedule schedule) : base(id)
    {
        Title = title;
        Description = description;
        Host = host;
        Schedule = schedule;
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
        Schedule schedule)
    {
        return new Activity(Guid.NewGuid(), title, description, host, schedule);
    }
}
