using app.infrastructure.Persistence;

namespace app.Extensions;
public static class SqliteExtensions
{
    public static void EnsureDatabaseCreated(this IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
        dbContext.Database.EnsureCreated();
    }
}
