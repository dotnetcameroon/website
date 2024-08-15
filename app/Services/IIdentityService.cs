using app.Models.Identity;
using ErrorOr;

namespace app.Services;
public interface IIdentityService
{
    Task<ErrorOr<User>> LoginAsync(string email, string password);
    Task LogoutAsync();
}
