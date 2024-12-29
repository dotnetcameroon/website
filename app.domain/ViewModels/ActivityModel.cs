using app.Models.EventAggregate.ValueObjects;

namespace app.domain.ViewModels;
public class ActivityModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public HostModel Host { get; set; } = new();
    public ActivitySchedule Schedule { get; set; } = new();
}
