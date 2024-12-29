namespace app.Models.EventAggregate.ValueObjects;

// Will be mapped to complex property
public class EventSchedule
{
    public DateTime Start { get; set; }
    public DateTime? End { get; set; }
    public bool IsAllDay { get; set; }

    private EventSchedule(DateTime start, DateTime? end, bool isAllDay)
    {
        Start = start;
        End = end;
        IsAllDay = isAllDay;
    }

    public EventSchedule()
    {
    }

    public static EventSchedule Create(DateTime start, DateTime? end, bool isAllDay = false)
    {
        var schedule = new EventSchedule(start, end, isAllDay);
        if (isAllDay)
        {
            schedule.Start = start.Date;
            schedule.End = null;
        }
        else
        {
            if (end == null || end <= start)
            {
                throw new ArgumentException("End date/time must be after the start date/time.");
            }

            schedule.Start = start;
            schedule.End = end;
        }

        schedule.IsAllDay = isAllDay;
        return schedule;
    }

    public void UpdateSchedule(DateTime start, DateTime? end = null, bool isAllDay = false)
    {
        if (isAllDay)
        {
            Start = start.Date;
            End = null;
        }
        else
        {
            if (end == null || end <= start)
            {
                throw new ArgumentException("End date/time must be after the start date/time.");
            }

            Start = start;
            End = end;
        }

        IsAllDay = isAllDay;
    }
}
