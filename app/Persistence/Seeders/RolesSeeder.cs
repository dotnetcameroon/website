using app.Utilities;
using EntityFrameworkCore.Seeder.Base;
using Microsoft.AspNetCore.Identity;

namespace app.Persistence.Seeders;
public class RolesSeeder(RoleManager<IdentityRole> roleManager) : ISeeder
{
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task SeedAsync()
    {
        await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
    }
}
