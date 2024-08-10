using System.Linq.Expressions;
using app.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace app.Persistence.Repositories.Base;
internal class Repository<TEntity, TId>(DbContext dbContext) : IRepository<TEntity, TId>
    where TEntity : Entity<TId>
{
    protected readonly DbContext _dbContext = dbContext;

    public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);

        return result.Entity;
    }

    public virtual Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _ = _dbContext.Set<TEntity>().Remove(entity);
        return Task.FromResult(true);
    }

    public virtual async Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default)
    {
        var records = await _dbContext
            .Set<TEntity>()
            .Where(x => x.Id!.Equals(id))
            .ExecuteDeleteAsync(cancellationToken);

        return records > 0;
    }

    public virtual Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Set<TEntity>()
            .AnyAsync(x => x.Id!.Equals(id), cancellationToken);
    }

    public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Set<TEntity>()
            .AnyAsync(expression, cancellationToken);
    }

    public virtual Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);
    }

    public virtual Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(expression, cancellationToken);
    }

    public virtual Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext
            .Set<TEntity>()
            .Update(entity);

        return Task.FromResult(entity);
    }
}
