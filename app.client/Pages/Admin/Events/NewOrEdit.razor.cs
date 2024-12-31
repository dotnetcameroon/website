using app.business.Services;
using app.domain.Models.EventAggregate;
using app.domain.Models.EventAggregate.Entities;
using app.domain.ViewModels;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace app.client.Pages.Admin.Events;
public partial class NewOrEdit
{
    [Parameter]
    public Guid? Id { get; set; }

    [SupplyParameterFromForm(FormName = "EventForm")]
    private EventModel EventModel { get; set; } = new();

    public List<Partner> AllPartners = [];
    public IEventService EventService { get; set; } = default!;
    public IPartnerService PartnerService { get; set; } = default!;
    public NavigationManager NavigationManager { get; set; } = default!;
    private IServiceScope _scope = default!;

    protected override async Task OnInitializedAsync()
    {
        InitializeDependencies();
        await LoadPartners();
        await LoadEvent();
        await base.OnInitializedAsync();
    }

    // Djoufson Che - 2024-12-31
    // We manually resolve the services through the service provider (which is a Singleton)
    // instead of dependency injection
    // because this is a client side component and we need to manually persist the scope
    // Note that this is the behavior I encountered, and this solution worked for me
    private void InitializeDependencies()
    {
        _scope = ServiceProvider.CreateScope();
        EventService = _scope.ServiceProvider.GetRequiredService<IEventService>();
        PartnerService = _scope.ServiceProvider.GetRequiredService<IPartnerService>();
        NavigationManager = _scope.ServiceProvider.GetRequiredService<NavigationManager>();
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
