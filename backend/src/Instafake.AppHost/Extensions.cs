namespace Instafake.AppHost;

internal static class Extensions
{
    extension(IResourceBuilder<ProjectResource> resource)
    {
        public IResourceBuilder<ProjectResource> WithKafkaEnvironment(IResourceBuilder<KafkaServerResource> kafka)
        {
            return resource.WithEnvironment(ctx =>
             {
                 var kafkaEndpoint = kafka.Resource.PrimaryEndpoint;
                 ctx.EnvironmentVariables.TryAdd("KAFKA__BOOTSTRAPSERVERS", $"{kafkaEndpoint.Host}:{kafkaEndpoint.Port}");
             });
        }
    }
}
