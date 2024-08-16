using app.Models.Identity;
using app.Utilities;
using EntityFrameworkCore.Seeder.Base;
using Microsoft.AspNetCore.Identity;

namespace app.Persistence.Seeders;
public class AdminsSeeder(UserManager<User> userManager) : ISeeder
{
    private readonly UserManager<User> _userManager = userManager;

    public async Task SeedAsync()
    {
        var admin = new User()
        {
            UserName = "admin",
            Email = "admin@mail.com",
            EmailConfirmed = true,

        };

        await _userManager.CreateAsync(admin, "Admin@123");
        await _userManager.AddToRoleAsync(admin, Roles.Admin);
    }
}
