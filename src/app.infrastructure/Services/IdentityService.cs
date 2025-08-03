using System.Security.Claims;
using app.business.Services;
using app.domain.Models.ExternalAppAggregate;
using app.domain.Models.Identity;
using app.infrastructure.Persistence.Repositories.Base;
using app.shared.Utilities;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace app.infrastructure.Services;
public class IdentityService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IPasswordHasher<Application> passwordHasher,
    IRepository<Application, Guid> repository,
    ITokenProvider tokenProvider) : IIdentityService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

    public async Task<ErrorOr<string>> LoginAppAsync(Guid applicationId, string applicationSecret)
    {
        var application = await repository.GetAsync(applicationId);
        if (application is null)
        {
            return Error.Unauthorized("Application.UnAuthorized", "Wrong credentials");
        }

        var passwordCheck = passwordHasher.VerifyHashedPassword(application, application.ClientSecret, applicationSecret);
        if (passwordCheck == PasswordVerificationResult.Failed)
        {
            return Error.Unauthorized("Application.UnAuthorized", "Wrong credentials");
        }

        return tokenProvider.Generate(
        [
            new(ClaimTypes.Name, application.Id.ToString()),
            new(ClaimTypes.NameIdentifier, application.Id.ToString()),
            new(ClaimTypes.Role, Roles.Application)
        ]);
    }

    public async Task<ErrorOr<User>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return Error.Unauthorized("User.UnAuthorized", "Wrong credentials");
        }

        var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordCheck)
        {
            return Error.Unauthorized("User.UnAuthorized", "Wrong credentials");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        return user;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
