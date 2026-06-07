using app.domain.Models.BannerAggregate.Enums;

namespace app.domain.ViewModels;

public class BannerModel
{
    public string MessageEn { get; set; } = string.Empty;
    public string MessageFr { get; set; } = string.Empty;
    public BannerVariant Variant { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Link { get; set; }
    public string? LinkLabelEn { get; set; }
    public string? LinkLabelFr { get; set; }
    public bool Dismissible { get; set; }
    public int Priority { get; set; }
    public bool IsEnabled { get; set; } = true;
}
