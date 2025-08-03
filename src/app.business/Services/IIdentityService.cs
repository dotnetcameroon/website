using app.domain.Models.Identity;
using ErrorOr;

namespace app.business.Services;
public interface IIdentityService
{
    Task<ErrorOr<User>> LoginAsync(string email, string password);
    Task<ErrorOr<string>> LoginAppAsync(Guid applicationId, string applicationSecret);
    Task LogoutAsync();
}
