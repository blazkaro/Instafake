using FluentResults;
using Instafake.BuildingBlocks.Domain;

namespace Instafake.BuildingBlocks.Application.Repositories;

public interface IWriteRepository<in TEntity>
    where TEntity : DomainEntity
{
    Task<Result> InsertAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
}
