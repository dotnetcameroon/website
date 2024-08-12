using EntityFrameworkCore.Seeder.Base;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCore.Seeder;

public class SeederEngine(ILogger<SeederEngine> logger)
{
    private readonly ILogger<SeederEngine> _logger = logger;

    public async Task<bool> RunAsync(string[] seederNames, ISeeder[] allSeeders)
    {
        var seeders = allSeeders.Where(s => seederNames.Contains(s.GetType().Name)).ToArray();
        return await RunAsync(seeders);
    }

    public async Task<bool> RunAsync(ISeeder[] seeders)
    {
        if (seeders.Length == 0)
        {
            _logger.LogInformation("No Seeders to apply.");
            return false;
        }

        _logger.LogInformation("Start seeding the database...");
        string seederName = string.Empty;
        bool appliedAny = false;
        try
        {
            foreach (var seeder in seeders)
            {
                seederName = seeder.GetType().Name;
                _logger.LogInformation("--> Applying Seeder: {SeederName}", seederName);
                await seeder.SeedAsync();
                _logger.LogInformation("--> Applied Seeder: {SeederName}", seederName);
                appliedAny = true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying the Seeder: {SeederName}", seederName);
        }
        return appliedAny;
    }
}
