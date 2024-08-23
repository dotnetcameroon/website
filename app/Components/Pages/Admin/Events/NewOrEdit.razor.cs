using app.Models.EventAggregate.Entities;
using app.Services;
using app.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace app.Components.Pages.Admin.Events;
public partial class NewOrEdit
{
    [Parameter]
    public Guid? Id { get; set; }

    [SupplyParameterFromForm(FormName = "EventForm")]
    private EventModel EventModel { get; set; } = new();

    public List<Partner> AllPartners = [];

    [Inject]
    public IEventService EventService { get; set; } = default!;

    [Inject]
    public IPartnerService PartnerService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadPartners();
        await LoadEvent();
        // await JSRuntime.InvokeVoidAsync("adjustTextareaHight", "description-input");
        await base.OnInitializedAsync();
    }

    private async Task LoadPartners()
    {
        var result = await PartnerService.GetAllAsync();
        if(result.IsError)
        {
            return;
        }

        AllPartners = [..result.Value];
    }

    private async Task LoadEvent()
    {
        if(Id is null)
        {
            return;
        }

        var result = await EventService.GetAsync(Id ?? Guid.Empty);
        if(result.IsError)
        {
            return;
        }

        var @event = result.Value;
        EventModel = new EventModel
        {
            Title = @event.Title,
            Description = @event.Description,
            Schedule = @event.Schedule,
            Type = @event.Type,
            Status = @event.Status,
            HostingModel = @event.HostingModel,
            Attendance = @event.Attendance,
            RegistrationLink = @event.RegistrationLink,
            ImageUrl = @event.ImageUrl,
            Images = @event.Images,
            Partners = [.. @event.Partners],
            Activities = @event.Activities.Select(a => new ActivityModel
            {
                Title = a.Title,
                Description = a.Description,
                Schedule = a.Schedule,
                Host = new HostModel
                {
                    Name = a.Host.Name,
                    Email = a.Host.Email,
                    ImageUrl = a.Host.ImageUrl
                }
            }).ToList()
        };
    }

    private void RemovePartner(Partner partner)
    {
        EventModel.Partners.Remove(partner);
        StateHasChanged();
    }

    private void RemoveActivity(ActivityModel activity)
    {
        EventModel.Activities.Remove(activity);
        StateHasChanged();
    }

    private void OnPartnerSelected(ChangeEventArgs args)
    {
        var partner = AllPartners.FirstOrDefault(p => p.Name.Equals(args.Value?.ToString() ?? string.Empty));
        if(partner is null)
        {
            return;
        }

        EventModel.Partners.Add(partner);
        StateHasChanged();
    }

    public void AddActivity(ActivityModel activity)
    {
        EventModel.Activities.Add(activity);
        StateHasChanged();
    }

    private static string GetActivityImage(ActivityModel model)
    {
        return 
            model.Host.ImageUrl ??
            model.Host.Base64Image ??
            string.Empty;
    }
}
