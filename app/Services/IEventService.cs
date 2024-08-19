using System.Linq.Expressions;
using app.Models.EventAggregate;
using app.Utilities;
using ErrorOr;

namespace app.Services;
public interface IEventService
{
    Task<ErrorOr<Event[]>> GetUpcomingAsync(int page = 1, int size = 2, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event[]>> GetPreviousAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> UpdateAsync(Event entity, CancellationToken cancellationToken = default);
    Task<ErrorOr<PagedList<Event>>> GetAllAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default);
    Task<ErrorOr<PagedList<Event>>> SearchAsync(Expression<Func<Event,bool>> predicate,int page = 1, int size = 5, CancellationToken cancellationToken = default);
    Task<ErrorOr<bool>> DeleteEvent(Guid id, CancellationToken cancellationToken = default);
}
