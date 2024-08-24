namespace app.Persistence;

public interface IUnitOfWork
{
    Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
