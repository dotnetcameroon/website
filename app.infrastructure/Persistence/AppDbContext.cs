using app.domain.Models.Common;
using app.domain.Models.EventAggregate;
using app.domain.Models.EventAggregate.Entities;
using app.domain.Models.Identity;
using app.infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace app.infrastructure.Persistence;
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
