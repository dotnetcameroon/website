using System.Linq.Expressions;
using app.business.Services;
using app.domain.Models.EventAggregate;
using app.domain.Models.EventAggregate.Entities;
using app.domain.Models.EventAggregate.Enums;
using app.domain.ViewModels;
using app.infrastructure.Persistence;
using app.infrastructure.Persistence.Repositories.Base;
using app.shared.Utilities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Host = app.domain.Models.EventAggregate.ValueObjects.Host;

namespace app.infrastructure.Services;
public class EventService(
    IUnitOfWork unitOfWork,
    IRepository<Event, Guid> eventRepository,
    IRepository<Activity, Guid> activityRepository,
    ICacheManager cacheManager) : IEventService
{
    private readonly IRepository<Event, Guid> _eventRepository = eventRepository;
    private readonly IRepository<Activity, Guid> _activityRepository = activityRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheManager _cacheManager = cacheManager;

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
        var cacheKey = $"event-{id}";
        var @event = await _cacheManager.GetOrCreateAsync(
            cacheKey,
            entry => _eventRepository
                .Table
                .Include(e => e.Activities.OrderBy(a => a.Schedule.Start))
                .Include(e => e.Partners)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken));

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
        var cacheKey = $"previous-events-{page}-{size}";
        var events = await _cacheManager.GetOrCreateAsync(
            cacheKey,
            e => _eventRepository
                .Table
                .Where(e => e.Status == EventStatus.Passed)
                .OrderByDescending(e => e.Schedule.Start)
                .Skip(page - 1)
                .Take(size)
                .ToArrayAsync(cancellationToken)
            );

        return events ?? [];
    }

    public async Task<ErrorOr<Event[]>> GetUpcomingAsync(int page = 1, int size = 2, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"upcoming-events-{page}-{size}";
        var events = await _cacheManager.GetOrCreateAsync(
            cacheKey,
            e => _eventRepository
                .Table
                .Where(e => e.Status == EventStatus.ComingSoon)
                .OrderByDescending(e => e.Schedule.Start)
                .Skip(page - 1)
                .Take(size)
                .ToArrayAsync(cancellationToken)
            );

        return events ?? [];
    }

    public async Task<ErrorOr<bool>> DeleteEvent(Guid id, CancellationToken cancellationToken = default)
    {
        return await _eventRepository.DeleteAsync(id, cancellationToken);
    }

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
        await _activityRepository.Table
            .Where(a => a.EventId == id)
            .ExecuteDeleteAsync(cancellationToken);

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
        ClearEventCache(id);
        return @event;
    }

    private void ClearEventCache(Guid id)
    {
        var keys = _cacheManager
            .GetKeys()
            .Where(k => k.Contains("events") || k.Contains(id.ToString()));

        foreach (var key in keys)
        {
            _cacheManager.Remove(key);
        }
    }

    public async Task<ErrorOr<Event>> CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default)
    {
        var @event = Event.Create(eventModel);
        @event = await _eventRepository.AddAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        ClearEventCache(@event.Id);
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
        ClearEventCache(id);
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
        ClearEventCache(id);
        return @event;
    }

    public async Task MarkPassedEventsAsync()
    {
        var now = DateTime.UtcNow;
        _ = await _eventRepository.Table
            .Where(e => (e.Schedule.End == null ? e.Schedule.Start < now.AddDays(1) : e.Schedule.End < now) && e.Status != EventStatus.Passed)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.Status, EventStatus.Passed));
    }
}
