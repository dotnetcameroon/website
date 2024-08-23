using System.Linq.Expressions;
using app.Models.EventAggregate;
using app.Models.EventAggregate.Enums;
using app.Persistence;
using app.Persistence.Repositories.Base;
using app.Utilities;
using app.ViewModels;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace app.Services.Impl;
internal class EventService(
    IRepository<Event, Guid> eventRepository,
    IUnitOfWork unitOfWork) : IEventService
{
    private readonly IRepository<Event, Guid> _eventRepository = eventRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

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

    public async Task<ErrorOr<Event>> UpdateAsync(Guid id, EventModel eventModel, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetAsync(id, cancellationToken);
        if (@event is null)
        {
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        @event.Update(eventModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
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
        if(@event is null)
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
        if(@event is null)
        {
            return Error.NotFound("Event.NotFound", "Event not found");
        }

        @event.Cancel();
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return @event;
    }
}
