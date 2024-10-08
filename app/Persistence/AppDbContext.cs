using app.Data.Interceptors;
using app.Models.Common;
using app.Models.EventAggregate;
using app.Models.EventAggregate.Entities;
using app.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence;
public sealed partial class AppDbContext(
    DbContextOptions<AppDbContext> options,
    DomainEventsInterceptor domainEventsInterceptor) : IdentityDbContext<User>(options), IDbContext
{
    private readonly DomainEventsInterceptor _domainEventsInterceptor = domainEventsInterceptor;
    public DbSet<Event> Events { get; set; }
    public DbSet<Partner> Partners { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<List<IDomainEvent>>();
        builder.ApplyConfigurationsFromAssembly(typeof(IDbContext).Assembly);
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_domainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    Task IDbContext.SaveChangesAsync(CancellationToken cancellationToken)
    {
        return SaveChangesAsync(cancellationToken);
    }
}
