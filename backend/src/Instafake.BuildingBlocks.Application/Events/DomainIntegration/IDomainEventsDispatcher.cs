using Instafake.BuildingBlocks.Domain;

namespace Instafake.BuildingBlocks.Application.Events.DomainIntegration;

public interface IDomainEventsDispatcher
{
    Task DispatchAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : DomainEntity;
}

