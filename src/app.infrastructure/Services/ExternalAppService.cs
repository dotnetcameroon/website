using app.business.Persistence;
using app.business.Services;
using app.domain.Models.ExternalAppAggregate;
using app.infrastructure.Persistence.Repositories.Base;
using app.shared.Utilities;
using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace app.infrastructure.Services;

public class ExternalAppService(
    IUnitOfWork unitOfWork,
    IRepository<Application, Guid> applicationRepository
) : IExternalAppService
{
    public async Task<ErrorOr<PagedList<Application>>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default)
    {
        var totalCount = await applicationRepository.Table.CountAsync(cancellationToken);
        var events = await applicationRepository
            .Table
            .OrderBy(e => e.ClientName)
            .Skip(page - 1)
            .Take(size)
            .ToListAsync(cancellationToken);

        return PagedList.FromList(events, totalCount, page, size);
    }

    public async Task<ErrorOr<(Application application, string secret)>> RegisterAsync(
        string applicationName,
        IPasswordHasher<Application> passwordHasher,
        CancellationToken cancellationToken = default)
    {
        var exists = await applicationRepository.ExistsAsync(a => a.ClientName == applicationName, cancellationToken);
        if (exists)
        {
            return Error.Conflict(
                "ExternalApplication.AlreadyExists",
                "The application you try to register already exists");
        }

        var application = Application.Create(applicationName);
        var clientSecret = application.UpdateClientSecret(passwordHasher);
        await applicationRepository.AddAsync(application, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return (application, clientSecret);
    }
}
