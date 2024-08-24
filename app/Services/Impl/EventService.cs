using System.Linq.Expressions;
using app.Models.EventAggregate;
using app.Models.EventAggregate.Entities;
using app.Models.EventAggregate.Enums;
using app.Persistence;
using app.Persistence.Repositories.Base;
using app.Utilities;
using app.ViewModels;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Host = app.Models.EventAggregate.ValueObjects.Host;

namespace app.Services.Impl;
internal class EventService(
    IUnitOfWork unitOfWork,
    IRepository<Event, Guid> eventRepository,
    IDbContext dbContext) : IEventService
{
    private readonly IRepository<Event, Guid> _eventRepository = eventRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDbContext _dbContext = dbContext;

    public Task<bool> ExistsAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _eventRepository.ExistsAsync(expression, cancellationToken);
    }

    public async Task<ErrorOr<PagedList<Event>>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var totalCount = await _eventRepository.Table.CountAsync(cancellationToken);
        var events = await _eventRepository
            .Table
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(size)
            .ToListAsync(cancellationToken);

        return PagedList.FromList(events, totalCount, page, size);
    }

    public async Task<ErrorOr<PagedList<Event>>> SearchAsync(Expression<Func<Event, bool>> predicate, int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var query = _eventRepository
            .Table
            .Include(e => e.Activities)
            .Include(e => e.Partners)
            .Where(predicate);

        var totalCount = await query.CountAsync(cancellationToken);
        var events = await query
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(size)
            .ToListAsync(cancellationToken);

        return PagedList.FromList(events, totalCount, page, size);
    }

    public async Task<ErrorOr<Event>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository
            .Table
            .Include(e => e.Activities)
            .Include(e => e.Partners)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (@event is null)
        {
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        return @event;
    }

    public async Task<ErrorOr<Event>> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetAsync(expression, cancellationToken);
        if (@event is null)
        {
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        return @event;
    }

    public async Task<ErrorOr<Event[]>> GetPreviousAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository
            .Table
            .Where(e => e.Status == EventStatus.Passed)
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(size)
            .ToArrayAsync(cancellationToken);

        return events;
    }

    public async Task<ErrorOr<Event[]>> GetUpcomingAsync(int page = 1, int size = 2, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository
            .Table
            .Where(e => e.Status == EventStatus.ComingSoon)
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(size)
            .ToArrayAsync(cancellationToken);

        return events;
    }

    public async Task<ErrorOr<bool>> DeleteEvent(Guid id, CancellationToken cancellationToken = default)
    {
        return await _eventRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<bool> UpdateAsync(Guid id, EventModel eventModel, CancellationToken cancellationToken = default)
    {
        var exists = await _eventRepository.Table.AnyAsync(e => e.Id == id, cancellationToken);
        if (!exists)
        {
            return false;
        }

        var transactionId = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        await _dbContext.Database.ExecuteSqlAsync(
            @$"
            UPDATE [Events]
            SET [Title] = {eventModel.Title},
                [Description] = {eventModel.Description},
                [Schedule_Start] = {eventModel.Schedule.Start},
                [Schedule_End] = {eventModel.Schedule.End},
                [Schedule_IsAllDay] = {eventModel.Schedule.IsAllDay},
                [Type] = {eventModel.Type},
                [Status] = {eventModel.Status},
                [HostingModel] = {eventModel.HostingModel},
                [Attendance] = {eventModel.Attendance},
                [RegistrationLink] = {eventModel.RegistrationLink},
                [ImageUrl] = {eventModel.ImageUrl},
                [Images] = {eventModel.Images}
            WHERE [Id] = {id};
            ",
            cancellationToken);

        // Delete the partners
        await _dbContext.Database.ExecuteSqlAsync(
            @$"
            DELETE FROM [EventPartner]
            WHERE [EventId] = {id};
            ",
            cancellationToken);

        // Insert the updated match for partners
        foreach (var partner in eventModel.Partners)
        {            
            await _dbContext.Database.ExecuteSqlAsync(
                @$"
                INSERT INTO [EventPartner] ([EventId], [PartnersId])
                VALUES ({id}, {partner.Id});
                ",
                cancellationToken);
        }

        // Delete the activities
        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            @$"
            DELETE FROM [Activities]
            WHERE [EventId] = {id};
            ",
            cancellationToken);

        // Insert the new activities
        foreach (var activity in eventModel.Activities)
        {
            await _dbContext.Database.ExecuteSqlAsync(
                @$"INSERT INTO [Activities]
                    ([Id], [Title], [Description], [EventId], [Host_Email], [Host_Name], [Host_ImageUrl], [Schedule_Start], [Schedule_End], [Schedule_IsAllDay])
                VALUES ({Guid.NewGuid()}, {activity.Title}, {activity.Description}, {id}, {activity.Host.Email}, {activity.Host.Name}, {activity.Host.ImageUrl}, {activity.Schedule.Start}, {activity.Schedule.End}, {activity.Schedule.IsAllDay});
                ",
                cancellationToken);
        }
        await _unitOfWork.CommitTransactionAsync(transactionId, cancellationToken);
        return true;
    }

    public async Task<ErrorOr<Event>> CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default)
    {
        var @event = Event.Create(eventModel);
        @event = await _eventRepository.AddAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return @event;
    }

    public async Task<ErrorOr<Event>> PublishAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetAsync(id, cancellationToken);
        if (@event is null)
        {
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        @event.Publish();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return @event;
    }

    public async Task<ErrorOr<Event>> CancelAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetAsync(id, cancellationToken);
        if (@event is null)
        {
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        @event.Cancel();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return @event;
    }
}
