using app.Models.Identity;
using ErrorOr;
using Microsoft.AspNetCore.Identity;

namespace app.Services.Impl;
public class IdentityService(UserManager<User> userManager, SignInManager<User> signInManager) : IIdentityService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;

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
