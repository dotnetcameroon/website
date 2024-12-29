using app.domain.Models.EventAggregate.Enums;

namespace app.Extensions;
public static class EventHostingModelExtensions
{
    public static string ToFriendlyString(this EventHostingModel hostingType)
    {
        return hostingType switch
        {
            EventHostingModel.InPerson => "In-Person",
            EventHostingModel.Online => "Online",
            EventHostingModel.Hybrid => "Hybrid",
            _ => "Hybrid"
        };
    }
}
