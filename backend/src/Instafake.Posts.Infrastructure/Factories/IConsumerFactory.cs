using Confluent.Kafka;

namespace Instafake.Posts.Infrastructure.Factories;

/// <summary>
/// The consumer-per-topic factory
/// </summary>
internal interface IConsumerFactory
{
    IConsumer<string, string> CreateUsersConsumer();
}
