using app.domain.Models.EventAggregate.ValueObjects;

namespace app.Extensions;
public static class ScheduleExtensions
{
    public static string ToFriendlyString(this EventSchedule schedule)
    {
        if (schedule.IsAllDay)
        {
            return $"{schedule.Start.ToString("hh:mm tt")} - All Day";
        }
        return $"{schedule.Start.ToString("hh:mm tt")} - {schedule.End?.ToString("hh:mm tt")}";
    }

    public static string ToFriendlyString(this ActivitySchedule schedule)
    {
        return $"{schedule.Start.ToString("hh:mm tt")} - {schedule.End.ToString("hh:mm tt")}";
    }

    public static string ToFriendlyDateString(this EventSchedule schedule)
    {
        int day = schedule.Start.Day;

        string suffix = day switch
        {
            11 or 12 or 13 => "th",
            _ => (day % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            }
        };

        return $"{day}{suffix} {schedule.Start:MMMM yyyy}";
    }
}
