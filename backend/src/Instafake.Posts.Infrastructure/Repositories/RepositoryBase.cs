using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Repositories;

internal abstract class RepositoryBase<TEntity, TDbContext>(TDbContext dbContext)
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
}
