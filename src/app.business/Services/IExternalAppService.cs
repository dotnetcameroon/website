using app.domain.Models.ExternalAppAggregate;
using app.shared.Utilities;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace app.business.Services;
public interface IExternalAppService
{
    Task<ErrorOr<PagedList<Application>>> GetAllAsync(int page = 1, int size = 10, CancellationToken cancellationToken = default);
    Task<ErrorOr<(Application application, string secret)>> RegisterAsync(string applicationName, IPasswordHasher<Application> passwordHasher, CancellationToken cancellationToken = default);
}
