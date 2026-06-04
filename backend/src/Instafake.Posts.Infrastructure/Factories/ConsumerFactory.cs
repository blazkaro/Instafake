using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Instafake.Posts.Infrastructure.Factories;

internal class ConsumerFactory(IConfiguration configuration) : IConsumerFactory
{
    private readonly IConfiguration _configuration = configuration;

    public IConsumer<string, string> CreateUsersConsumer() => CreateConsumer("UsersConsumer");

    private IConsumer<string, string> CreateConsumer(string configSection)
    {
        var config = CreateConsumerConfig(configSection);
        return new ConsumerBuilder<string, string>(config).Build();
    }

    private ConsumerConfig CreateConsumerConfig(string configSection)
    {
        var kafka = _configuration.GetRequiredSection("Kafka");
        var consumer = kafka.GetRequiredSection(configSection);
        var config = new ConsumerConfig();

        kafka.Bind(config);
        consumer.Bind(config);

        return config;
    }
}
