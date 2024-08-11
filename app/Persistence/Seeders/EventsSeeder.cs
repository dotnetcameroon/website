using app.Persistence.Factories;
using EntityFrameworkCore.Seeder.Base;

namespace app.Persistence.Seeders;
public class EventsSeeder(AppDbContext dbContext, ILogger<EventsSeeder> logger) : ISeeder
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly ILogger<EventsSeeder> _logger = logger;

    public async Task SeedAsync()
    {
        var events = new EventsFactory().Generate(10);
        foreach (var @event in events)
        {
            _logger.LogInformation("Event created: {Title}", @event.Title);
        }

        await Task.CompletedTask;
        // await _dbContext.AddRangeAsync(events);
        // await _dbContext.SaveChangesAsync();
    }
}
