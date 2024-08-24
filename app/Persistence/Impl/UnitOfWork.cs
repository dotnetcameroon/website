using System.Collections.Concurrent;
using app.Models.EventAggregate.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace app.Persistence.Impl;
internal class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    private readonly DbContext _dbContext = dbContext;
    private static readonly ConcurrentDictionary<Guid, IDbContextTransaction> _transactions = [];

    public async Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transactionId = Guid.NewGuid();
        _transactions[transactionId] = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return transactionId;
    }

    public async Task CommitTransactionAsync(Guid transactionId, CancellationToken cancellationToken = default)
    {
        if (_transactions.TryRemove(transactionId, out var transaction))
        {
            await transaction.CommitAsync(cancellationToken);
            await transaction.DisposeAsync();
            return;
        }

        throw new InvalidOperationException("Transaction not found.");
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                if (entry.Entity is Activity)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();
                    if(databaseValues is null)
                    {
                        return;
                    }

                    foreach (var property in entry.CurrentValues.Properties)
                    {
                        var proposedValue = entry.CurrentValues[property];
                        var databaseValue = databaseValues[property];
                        entry.CurrentValues[property] = proposedValue;
                    }

                    entry.OriginalValues.SetValues(databaseValues);
                }
                else
                {
                    throw new NotSupportedException(
                        "Don't know how to handle concurrency conflicts for "
                        + entry.Metadata.Name);
                }
            }
        }
    }
}
