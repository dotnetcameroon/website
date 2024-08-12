using System.Linq.Expressions;
using app.Models.EventAggregate;
using app.Models.EventAggregate.Enums;
using app.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace app.Services.Impl;
internal class EventService(IRepository<Event, Guid> eventRepository) : IEventService
{
    private readonly IRepository<Event, Guid> _eventRepository = eventRepository;

    public Task<bool> ExistsAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _eventRepository.ExistsAsync(expression, cancellationToken);
    }

    public async Task<Event[]> GetAllAsync(int page = 1, int number = 5, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository
            .Table
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(number)
            .ToArrayAsync(cancellationToken);

        return events;
    }

    public async Task<Event[]> GetAllAsync(Expression<Func<Event, bool>> predicate, int page = 1, int number = 5, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository
            .Table
            .Include(e => e.Activities)
            .Include(e => e.Partners)
            .Where(predicate)
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(number)
            .ToArrayAsync(cancellationToken);

        return events;
    }

    public Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _eventRepository.GetAsync(id, cancellationToken);
    }

    public Task<Event?> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _eventRepository.GetAsync(expression, cancellationToken);
    }

    public async Task<Event[]> GetPreviousAsync(int page = 1, int number = 5, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository
            .Table
            .Where(e => e.Status == EventStatus.Passed)
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(number)
            .ToArrayAsync(cancellationToken);

        return events;
    }

    public async Task<Event[]> GetUpcomingAsync(int page = 1, int number = 2, CancellationToken cancellationToken = default)
    {
        var events = await _eventRepository
            .Table
            .Where(e => e.Status == EventStatus.CommingSoon)
            .OrderByDescending(e => e.Schedule.Start)
            .Skip(page - 1)
            .Take(number)
            .ToArrayAsync(cancellationToken);

        return events;
    }

    public Task<Event> UpdateAsync(Event entity, CancellationToken cancellationToken = default)
    {
        return _eventRepository.UpdateAsync(entity, cancellationToken);
    }
}
