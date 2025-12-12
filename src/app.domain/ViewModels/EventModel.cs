using app.domain.Models.EventAggregate.Entities;
using app.domain.Models.EventAggregate.Enums;
using app.domain.Models.EventAggregate.ValueObjects;

namespace app.domain.ViewModels;

public class EventModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public EventSchedule Schedule { get; set; } = new();
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public EventHostingModel HostingModel { get; set; }
    public int Attendance { get; set; }
    public string RegistrationLink { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Images { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;
    public List<Partner> Partners { get; set; } = [];
    public List<ActivityModel> Activities { get; set; } = [];
}
