using app.Models.EventAggregate.Entities;
using EntityFrameworkCore.Seeder.Base;

namespace app.Persistence.Seeders;
public class PartnersSeeder(AppDbContext dbContext) : ISeeder
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task SeedAsync()
    {
        var dotnetFoundation = Partner.Create(".NET Foundation", "/assets/sponsors/dotnetfondation.webp", "https://dotnetfoundation.org");
        var jetbrains = Partner.Create("JetBrains", "/assets/sponsors/jetbrainslogo.png", "https://www.jetbrains.com");
        var infiniteSolutions = Partner.Create("Infinite Solutions", "/assets/sponsors/is-logo.png", "https://www.isolutions-intl.com/");

        await _dbContext.AddRangeAsync(dotnetFoundation, jetbrains, infiniteSolutions);
        await _dbContext.SaveChangesAsync();
    }
}
