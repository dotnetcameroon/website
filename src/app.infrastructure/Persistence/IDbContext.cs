using app.domain.Models.EventAggregate;
using app.domain.Models.EventAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace app.infrastructure.Persistence;
public interface IDbContext
{
    DatabaseFacade Database { get; }
    public DbSet<Event> Events { get; }
    public DbSet<Partner> Partners { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
