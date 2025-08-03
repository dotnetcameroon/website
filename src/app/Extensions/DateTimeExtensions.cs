namespace app.Extensions;
public static class DateTimeExtensions
{
    public static string ToCustomDateString(this DateTime dateTime)
    {
        int day = dateTime.Day;

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

        return $"{day}{suffix} {dateTime:MMMM yyyy}";
    }
}
