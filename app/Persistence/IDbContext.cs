using app.Models.EventAggregate;
using app.Models.EventAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace app.Persistence;
public interface IDbContext
{
    DatabaseFacade Database { get; }
    public DbSet<Event> Events { get; }
    public DbSet<Partner> Partners { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
