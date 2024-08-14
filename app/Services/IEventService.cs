using System.Linq.Expressions;
using app.Models.EventAggregate;
using ErrorOr;

namespace app.Services;
public interface IEventService
{
    Task<ErrorOr<Event[]>> GetUpcomingAsync(int page = 1, int number = 2, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event[]>> GetPreviousAsync(int page = 1, int number = 5, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> UpdateAsync(Event entity, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event[]>> GetAllAsync(int page = 1, int number = 5, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event[]>> GetAllAsync(Expression<Func<Event,bool>> predicate,int page = 1, int number = 5, CancellationToken cancellationToken = default);
}
