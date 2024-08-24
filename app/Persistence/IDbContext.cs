using Microsoft.EntityFrameworkCore.Infrastructure;

namespace app.Persistence;
public interface IDbContext
{
    DatabaseFacade Database { get; }
}
