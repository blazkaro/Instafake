using Instafake.BuildingBlocks.Domain;
using MediatR;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Instafake.BuildingBlocks.Application.Events.DomainIntegration;

internal class DomainEventsDispatcher(IPublisher publisher) : IDomainEventsDispatcher
{
    private readonly IPublisher _publisher = publisher;

    private static readonly ConcurrentDictionary<Type, Func<IDomainEvent, INotification>> NotificationFactoryCache = new();

    public async Task DispatchAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : DomainEntity
    {
        // Events have to be processed in order so do not think about Task.WhenAll etc
        foreach (var ev in entity.Events)
        {
            var factory = NotificationFactoryCache.GetOrAdd(ev.GetType(), CreateFactory);

            var notification = factory(ev);
            await _publisher.Publish(notification, cancellationToken);
        }
    }

    // Runs only once per unique (never processed till now) event type
    private static Func<IDomainEvent, INotification> CreateFactory(Type eventType)
    {
        var notificationType = typeof(DomainEventNotification<>)
            .MakeGenericType(eventType);

        var ctor = notificationType.GetConstructor([eventType])
            ?? throw new InvalidOperationException($"Constructor not found for {notificationType.Name}");

        var parameter = Expression.Parameter(typeof(IDomainEvent));

        var body = Expression.New(
            ctor,
            Expression.Convert(parameter, eventType));

        return Expression.Lambda<Func<IDomainEvent, INotification>>(
                Expression.Convert(body, typeof(INotification)),
                parameter)
            .Compile();
    }
}