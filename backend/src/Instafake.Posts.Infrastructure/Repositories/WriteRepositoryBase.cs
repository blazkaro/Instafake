using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Instafake.Posts.Infrastructure.Repositories;

internal abstract class WriteRepositoryBase<TEntity, TDbContext>(TDbContext dbContext)
    where TEntity : class
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext = dbContext;

    public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task BatchDeleteAsync(Expression<Func<TEntity, bool>> where, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>()
            .Where(where)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
