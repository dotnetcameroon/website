using app.Models.EventAggregate.ValueObjects;

namespace app.Extensions;
public static class ScheduleExtensions
{
    public static string ToFriendlyString(this Schedule schedule)
    {
        if (schedule.IsAllDay)
        {
            return $"{schedule.Start.ToString("hh:mm tt")} - All Day";
        }
        return $"{schedule.Start.ToString("hh:mm tt")} - {schedule.End?.ToString("hh:mm tt")}";
    }
}
