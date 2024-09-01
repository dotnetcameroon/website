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
    IDbContext dbContext,
    IUnitOfWork unitOfWork,
    IRepository<Event, Guid> eventRepository,
    IRepository<Activity, Guid> activityRepository) : IEventService
{
    private readonly IRepository<Event, Guid> _eventRepository = eventRepository;
    private readonly IRepository<Activity, Guid> _activityRepository = activityRepository;
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
            .Include(e => e.Activities.OrderBy(a => a.Schedule.Start))
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

    // This method uses a mix and match of Ef queries qnd raw sql queries
    public async Task<ErrorOr<Event>> UpdateAsync(Guid id, EventModel eventModel, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetAsync(e => e.Id == id, cancellationToken);
        if (@event is null)
        {
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        var transactionId = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        @event.UpdateInfos(eventModel);
        @event.UpdatePartners(eventModel.Partners);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Delete the activities
        await _dbContext.Database.ExecuteSqlInterpolatedAsync(
            @$"
            DELETE FROM [Activities]
            WHERE [EventId] = {id};
            ",
            cancellationToken);

        // Insert the new activities
        var activities = eventModel.Activities
            .Select(activity => 
                Activity.Create(
                    activity.Title,
                    activity.Description,
                    Host.Create(
                        activity.Host.Name,
                        activity.Host.Email,
                        activity.Host.ImageUrl),
                    activity.Schedule,
                    @event));
        await _activityRepository.AddRangeAsync(activities, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(transactionId, cancellationToken);
        return @event;
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

    public async Task MarkPassedEventsAsync()
    {
        var passedEvents = await _eventRepository.Table
            .Where(e => e.Schedule.End < DateTime.UtcNow && e.Status != EventStatus.Passed)
            .ToArrayAsync();

        foreach (var @event in passedEvents)
        {
            @event.MarkAsPassed();
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
