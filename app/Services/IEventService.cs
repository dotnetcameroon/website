using System.Linq.Expressions;
using app.Models.EventAggregate;

namespace app.Services;
public interface IEventService
{
    Task<Event[]> GetUpcomingAsync(int page = 1, int number = 2, CancellationToken cancellationToken = default);
    Task<Event[]> GetPreviousAsync(int page = 1, int number = 5, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Event?> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<Event> UpdateAsync(Event entity, CancellationToken cancellationToken = default);
    Task<Event[]> GetAllAsync(int page = 1, int number = 5, CancellationToken cancellationToken = default);
    Task<Event[]> GetAllAsync(Expression<Func<Event,bool>> predicate,int page = 1, int number = 5, CancellationToken cancellationToken = default);
}
