namespace app.Models.EventAggregate.ValueObjects;

// Will be mapped to complex property
public class ActivitySchedule
{
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }

    private ActivitySchedule(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;
    }

    public ActivitySchedule()
    {
    }

    public static ActivitySchedule Create(TimeOnly start, TimeOnly end)
    {
        var schedule = new ActivitySchedule(start, end);
        return schedule;
    }
}
