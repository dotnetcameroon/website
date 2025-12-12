using app.infrastructure.Persistence.Factories;
using EntityFrameworkCore.Seeder.Base;

namespace app.infrastructure.Persistence.Seeders;

public class ProjectsSeeder(ProjectDbContext context) : ISeeder
{
    public async Task SeedAsync()
    {
        var projects = new ProjectsFactory().Generate(5);
        await context.Projects.AddRangeAsync(projects);
        await context.SaveChangesAsync();
    }
}
