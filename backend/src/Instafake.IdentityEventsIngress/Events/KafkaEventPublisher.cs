using Confluent.Kafka;
using Instafake.IdentityEventsIngress.Retry;
using System.Text;
using System.Text.Json;

namespace Instafake.IdentityEventsIngress.Events;

public class KafkaEventPublisher<TEvent>(IProducer<string, string> producer, RetryQueue<TEvent> retryQueue) : IEventPublisher<TEvent>
    where TEvent : EventBase, new()
{
    private const string TOPIC = "users";

    private readonly IProducer<string, string> _producer = producer;
    private readonly RetryQueue<TEvent> _retryQueue = retryQueue;

    public async Task PublishAsync(TEvent ev, CancellationToken cancellationToken = default)
    {
        try
        {
            var headers = new Headers()
            {
                new Header("Event-Type", Encoding.UTF8.GetBytes(ev.EventType))
            };

            await _producer.ProduceAsync(TOPIC, new() { Key = ev.UserId, Value = JsonSerializer.Serialize(ev), Headers = headers }, cancellationToken);
        }
        catch
        {
            _retryQueue.Enqueue(ev);
        }
    }
}
