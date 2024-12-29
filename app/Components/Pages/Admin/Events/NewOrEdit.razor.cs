using app.domain.ViewModels;
using app.Models.EventAggregate;
using app.Models.EventAggregate.Entities;
using app.Models.EventAggregate.ValueObjects;
using app.Persistence;
using app.Services;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public IUnitOfWork UnitOfWork { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadPartners();
        await LoadEvent();
        // await JSRuntime.InvokeVoidAsync("adjustTextareaHight", "description-input");
        await base.OnInitializedAsync();
    }

    private async Task HandleSubmit()
    {
        ErrorOr<Event> result;
        if (Id is null)
        {
            result = await EventService.CreateAsync(EventModel);
        }
        else
        {
            result = await EventService.UpdateAsync(Id.Value, EventModel);
        }

        if (result.IsError)
        {
            NavigationManager.NavigateTo("/errors", true);
            return;
        }

        NavigationManager.NavigateTo("/admin", true);
    }

    private async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file is null)
        {
            return;
        }

        using var memoryStream = new MemoryStream();
        await file.OpenReadStream().CopyToAsync(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();

        EventModel.ImageUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(fileBytes)}";
        StateHasChanged();
    }

    private async Task CancelEvent(Guid eventId)
    {
        var result = await EventService.CancelAsync(eventId);
        if (result.IsError)
        {
            NavigationManager.NavigateTo("/errors", true);
        }
        else
        {
            NavigationManager.NavigateTo("/admin", true);
        }
    }

    private async Task PublishEvent(Guid eventId)
    {
        var result = await EventService.PublishAsync(eventId);
        if (result.IsError)
        {
            NavigationManager.NavigateTo("/errors", true);
        }
        else
        {
            NavigationManager.NavigateTo("/admin", true);
        }
    }

    private async Task LoadPartners()
    {
        var result = await PartnerService.GetAllAsync();
        if (result.IsError)
        {
            return;
        }

        AllPartners = [.. result.Value];
    }

    private async Task LoadEvent()
    {
        if (Id is null)
        {
            return;
        }

        var result = await EventService.GetAsync(Id ?? Guid.Empty);
        if (result.IsError)
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
        if (partner is null)
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
            model.Host.ImageUrl;
    }
}
