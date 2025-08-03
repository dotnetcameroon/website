using System.Linq.Expressions;
using app.domain.Models.EventAggregate;
using app.domain.ViewModels;
using app.shared.Utilities;
using ErrorOr;

namespace app.business.Services;
public interface IEventService
{
    Task<ErrorOr<Event[]>> GetUpcomingAsync(int page = 1, int size = 2, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event[]>> GetPreviousAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> GetAsync(Expression<Func<Event, bool>> expression, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> UpdateAsync(Guid id, EventModel eventModel, CancellationToken cancellationToken = default);
    Task<ErrorOr<PagedList<Event>>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default);
    Task<ErrorOr<PagedList<Event>>> SearchAsync(Expression<Func<Event,bool>> predicate,int page = 1, int size = 10, CancellationToken cancellationToken = default);
    Task<ErrorOr<bool>> DeleteEvent(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> PublishAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Event>> CancelAsync(Guid id, CancellationToken cancellationToken = default);
    Task MarkPassedEventsAsync();
}
