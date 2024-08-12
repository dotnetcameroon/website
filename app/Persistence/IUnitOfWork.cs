namespace app.Persistence;

internal interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
