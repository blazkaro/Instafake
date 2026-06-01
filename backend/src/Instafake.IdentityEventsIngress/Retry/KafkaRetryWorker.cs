using Confluent.Kafka;
using Instafake.IdentityEventsIngress.Events;

namespace Instafake.IdentityEventsIngress.Retry;

public class KafkaRetryWorker<TEvent>(RetryQueue<TEvent> retryQueue, IEventPublisher<TEvent> publisher) : BackgroundService
    where TEvent : EventBase, new()
{
    private const int RETRIES = 3;

    private readonly RetryQueue<TEvent> _retryQueue = retryQueue;
    private readonly IEventPublisher<TEvent> _publisher = publisher;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var ev in _retryQueue.ReadAll().WithCancellation(stoppingToken))
        {
            for (int i = 0; i < RETRIES; i++)
            {
                try
                {
                    await _publisher.PublishAsync(ev, stoppingToken);
                    break;
                }
                catch (KafkaException)
                {
                    await Task.Delay(3000 * (i + 1), stoppingToken); // problem with kafka, wait...
                    // TODO: identity events are essential for further workflow, some persistence for failed events in order is needed to avoid losing these events
                }
            }
        }
    }
}
