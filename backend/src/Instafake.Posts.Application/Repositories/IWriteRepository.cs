using Instafake.Posts.Domain.Entities;

namespace Instafake.Posts.Application.Repositories;

public interface IWriteRepository<in TEntity>
    where TEntity : DomainEntity
{
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
