using app.Models.EventAggregate.Entities;
using app.Services;
using app.ViewModels;
using Microsoft.AspNetCore.Components;

namespace app.Components.Pages.Admin.Events;
public partial class NewOrEdit
{
    [Parameter]
    public Guid? Id { get; set; }

    [SupplyParameterFromForm(FormName = "EventForm")]
    private EventModel Model { get; set; } = new();

    public List<Partner> AllPartners = [];

    [Inject]
    public IEventService EventService { get; set; } = default!;

    [Inject]
    public IPartnerService PartnerService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadPartners();
        await LoadEvent();
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
        Model = new EventModel
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
            Activities = [.. @event.Activities]
        };
    }

    private void RemovePartner(Partner partner)
    {
        Model.Partners.Remove(partner);
        StateHasChanged();
    }

    private void OnPartnerSelected(ChangeEventArgs args)
    {
        var partner = AllPartners.FirstOrDefault(p => p.Name.Equals(args.Value?.ToString() ?? string.Empty));
        if(partner is null)
        {
            return;
        }

        Model.Partners.Add(partner);
        StateHasChanged();
    }
}
