using app.business.Persistence;
using app.business.Services;
using app.domain.Models.BannerAggregate;
using app.domain.ViewModels;
using app.infrastructure.Persistence.Repositories.Base;
using app.shared.Utilities;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace app.infrastructure.Services;

public class BannerService(
    IUnitOfWork unitOfWork,
    IRepository<Banner, Guid> bannerRepository,
    ICacheManager cacheManager) : IBannerService
{
    private const string ActiveBannersCacheKey = "banners-active";

    public async Task<ErrorOr<PagedList<Banner>>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var totalCount = await bannerRepository.Table.CountAsync(cancellationToken);
        var banners = await bannerRepository
            .Table
            .OrderByDescending(b => b.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return PagedList.FromList(banners, totalCount, page, size);
    }

    public async Task<ErrorOr<Banner>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var banner = await bannerRepository.GetAsync(id, cancellationToken);
        if (banner is null)
        {
            return Error.NotFound("Banner.NotFound", "Banner not found");
        }

        return banner;
    }

    public async Task<Banner[]> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var banners = await cacheManager.GetOrCreateAsync(
            ActiveBannersCacheKey,
            entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                var now = DateTime.UtcNow;
                return bannerRepository
                    .Table
                    .Where(b => b.IsEnabled && b.StartDate <= now && b.EndDate >= now)
                    .OrderByDescending(b => b.Priority)
                    .ThenBy(b => b.CreatedAt)
                    .ToArrayAsync(cancellationToken);
            });

        return banners ?? [];
    }

    public async Task<ErrorOr<Banner>> CreateAsync(BannerModel model, CancellationToken cancellationToken = default)
    {
        if (model.EndDate <= model.StartDate)
        {
            return Error.Validation("Banner.InvalidSchedule", "End date must be after start date");
        }

        var banner = Banner.Create(model);
        banner = await bannerRepository.AddAsync(banner, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        ClearBannerCache();
        return banner;
    }

    public async Task<ErrorOr<Banner>> UpdateAsync(Guid id, BannerModel model, CancellationToken cancellationToken = default)
    {
        if (model.EndDate <= model.StartDate)
        {
            return Error.Validation("Banner.InvalidSchedule", "End date must be after start date");
        }

        var banner = await bannerRepository.GetAsync(id, cancellationToken);
        if (banner is null)
        {
            return Error.NotFound("Banner.NotFound", "Banner not found");
        }

        banner.Update(model);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        ClearBannerCache();
        return banner;
    }

    public async Task<ErrorOr<bool>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var deleted = await bannerRepository.DeleteAsync(id, cancellationToken);
        if (deleted)
        {
            ClearBannerCache();
        }
        return deleted;
    }

    private void ClearBannerCache()
    {
        var keys = cacheManager.GetKeys().Where(k => k.Contains("banners"));
        foreach (var key in keys)
        {
            cacheManager.Remove(key);
        }
    }
}
