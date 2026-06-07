using app.domain.Models.BannerAggregate;
using app.domain.ViewModels;
using app.shared.Utilities;
using ErrorOr;

namespace app.business.Services;

public interface IBannerService
{
    Task<ErrorOr<PagedList<Banner>>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default);
    Task<ErrorOr<Banner>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Banner[]> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<ErrorOr<Banner>> CreateAsync(BannerModel model, CancellationToken cancellationToken = default);
    Task<ErrorOr<Banner>> UpdateAsync(Guid id, BannerModel model, CancellationToken cancellationToken = default);
    Task<ErrorOr<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
