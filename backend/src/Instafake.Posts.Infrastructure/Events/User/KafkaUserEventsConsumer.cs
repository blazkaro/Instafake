using Confluent.Kafka;
using Instafake.Posts.Application.Events;
using Instafake.Posts.Infrastructure.Factories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Text.Json;

namespace Instafake.Posts.Infrastructure.Events.User;

internal class KafkaUserEventsConsumer : IEventsConsumer
{
    private const string TOPIC = "users";

    private readonly IConsumer<string, string> _consumer;
    private readonly IServiceScopeFactory _scopeFactory;

    public KafkaUserEventsConsumer(IConsumerFactory consumerFactory, IServiceScopeFactory scopeFactory)
    {
        _consumer = consumerFactory.CreateUsersConsumer();
        _scopeFactory = scopeFactory;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(TOPIC);

        while (!cancellationToken.IsCancellationRequested)
        {
            ConsumeResult<string, string>? result = null;
            try
            {
                result = _consumer.Consume(cancellationToken);
                if (result is null)
                    continue;

                var eventType = Encoding.UTF8.GetString(result.Message.Headers.GetLastBytes("Event-Type"));
                var ev = ReadEvent(eventType, result.Message);
                if (ev is null)
                    continue; // unknown event (we don't need to handle it)

                // TODO: check if event was processed (store processed events). Otherwise exception may be thrown (e.g. creating existing entity) and we will retry infinitely

                await using var scope = _scopeFactory.CreateAsyncScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(ev, cancellationToken);
            }
            catch (JsonException)
            {
                // invalid message, skip it
            }
            catch (KeyNotFoundException)
            {
                // missing header, skip it
            }
            catch (TaskCanceledException)
            {
                // loop will exit on next iteration
            }
            catch
            {
                // Possible invalid scenario: user.created failed, but user.updated was processed
                // Solution: Wait 5 seconds and retry the same message to avoid processing messages in bad order.

                if (result is null)
                    continue;

                RetryConsume(result, cancellationToken);
            }
        }
    }

    private void RetryConsume(ConsumeResult<string, string> result, CancellationToken cancellationToken)
    {
        _consumer.Pause(_consumer.Assignment);

        // Keep the consumer alive by polling null heartbeats for 5 seconds
        var waitEndTime = DateTime.UtcNow.AddSeconds(5);
        while (DateTime.UtcNow < waitEndTime && !cancellationToken.IsCancellationRequested)
        {
            // consumer is paused, so we need to provide a timeout to Consume() to avoid blocking infinitely
            _consumer.Consume(TimeSpan.FromMilliseconds(500));
        }

        _consumer.Resume(_consumer.Assignment);
        _consumer.Seek(result.TopicPartitionOffset);
    }

    /// <summary>
    /// Reads the event from the message based on the event type
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="message"></param>
    /// <returns>The event, otherwise if the event type is not recognized, it returns null.</returns>
    private static object? ReadEvent(string eventType, Message<string, string> message) =>
        eventType switch
        {
            IdentityEvents.UserCreated => JsonSerializer.Deserialize<AuthorCreatedEvent>(message.Value),
            _ => null
        };
}
