using System.Linq.Expressions;
using app.domain.Models.EventAggregate.Entities;
using app.shared.Utilities;
using ErrorOr;

namespace app.business.Services;
public interface IPartnerService
{
    Task<ErrorOr<Partner>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Partner>> CreateAsync(Partner partner, CancellationToken cancellationToken = default);
    Task<ErrorOr<Partner>> GetAsync(Expression<Func<Partner, bool>> predicate, CancellationToken cancellationToken = default);
    Task<ErrorOr<Partner>> UpdateAsync(Partner partner, CancellationToken cancellationToken = default);
    Task<ErrorOr<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ErrorOr<Partner[]>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<PagedList<Partner>>> GetPagedAsync(int page = 1, int size = 5, CancellationToken cancellationToken = default);
}
