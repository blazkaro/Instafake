using Confluent.Kafka;
using Instafake.Profiles.Application;
using Instafake.Profiles.Application.Integration.Outgoing;
using Instafake.Profiles.Application.Options;
using Instafake.Profiles.Application.Repositories;
using Instafake.Profiles.Domain.Events;
using Instafake.Profiles.Infrastructure.DbContexts;
using Instafake.Profiles.Infrastructure.Events.Self;
using Instafake.Profiles.Infrastructure.Interceptors;
using Instafake.Profiles.Infrastructure.Repositories;
using Instafake.Profiles.Infrastructure.Services;
using JasperFx.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Kafka;
using Wolverine.Postgresql;

namespace Instafake.Profiles.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
        {
            services.AddDbContextWithWolverineIntegration<ProfilesDbContext>((serviceProvider, cfg) =>
            {
                var interceptor = serviceProvider.GetRequiredService<SequenceInterceptor>();
                cfg.UseNpgsql(configuration.GetConnectionString("profiles-api-db"))
                   .AddInterceptors(interceptor);
            });

            services.AddScoped<IWriteRepository<Domain.Entities.Profile>, ProfileWriteRepository>();
            services.AddScoped<IWriteRepository<Domain.Entities.Follow>, FollowWriteRepository>();

            services.AddSingleton<IProfileSequenceAllocator, ProfileSequenceAllocator>();
            services.AddSingleton<SequenceInterceptor>();

            services.Configure<FollowBucketOptions>(cfg =>
            {
                cfg.BucketSize = 5000;
            });

            return services;
        }
    }

    extension(IHostBuilder builder)
    {
        public void ConfigureInfrastructure(IConfiguration configuration)
        {
            builder.UseWolverine(options =>
            {
                options.UseRuntimeCompilation();

                options.ConfigureApplication();
                options.Discovery.IncludeAssembly(typeof(DependencyInjection).Assembly);

                options.PersistMessagesWithPostgresql(configuration.GetConnectionString("profiles-api-db"));

                options.Policies.AutoApplyTransactions();

                options.UseEntityFrameworkCoreTransactions();

                var kafkaHost = configuration.GetRequiredSection("Kafka:Host").Get<ConsumerConfig>()!;
                options.UseKafka(kafkaHost.BootstrapServers)
                    .AutoProvision();

                var kafkaDefaultConsumer = configuration.GetSection("Kafka:Consumers:Default").Get<ConsumerConfig>() ?? new ConsumerConfig();

                options.LocalQueue("follow-events");

                options.LocalQueue("profile-events")
                    .UseDurableInbox();

                options.PublishMessage<ProfileCreatedEvent>()
                    .ToLocalQueue("profile-events");

                options.PublishMessage<FollowCreatedEvent>()
                    .ToLocalQueue("follow-events");

                options.PublishMessage<FollowDeletedEvent>()
                    .ToLocalQueue("follow-events");

                options.PublishMessage<NotificationFanoutNext>()
                    .ToKafkaTopic("post-fanout-notifications")
                    .UseDurableOutbox();

                options.PublishMessage<PostNotification>()
                    .ToKafkaTopic("post-notifications")
                    .UseDurableOutbox();

                var usersConsumer = new ConsumerConfig(kafkaDefaultConsumer);
                configuration.GetSection("Kafka:Consumers:Users").Bind(usersConsumer);

                options.ListenToKafkaTopic("users")
                    .ConfigureConsumer(cfg =>
                    {
                        foreach (var keyPair in usersConsumer)
                        {
                            cfg.Set(keyPair.Key, keyPair.Value);
                        }
                    }).UseDurableInbox();

                var postsConsumer = new ConsumerConfig(kafkaDefaultConsumer);
                configuration.GetSection("Kafka:Consumers:Posts").Bind(postsConsumer);

                options.ListenToKafkaTopic("post-events")
                    .ConfigureConsumer(cfg =>
                    {
                        foreach (var keyPair in postsConsumer)
                        {
                            cfg.Set(keyPair.Key, keyPair.Value);
                        }
                    }).UseDurableInbox();

                var postFanoutNotificationsConsumer = new ConsumerConfig(kafkaDefaultConsumer);
                configuration.GetSection("Kafka:Consumers:PostFanoutNotifications").Bind(postFanoutNotificationsConsumer);

                options.ListenToKafkaTopic("post-fanout-notifications")
                    .ConfigureConsumer(cfg =>
                    {
                        foreach (var keyPair in postFanoutNotificationsConsumer)
                        {
                            cfg.Set(keyPair.Key, keyPair.Value);
                        }
                    }).UseDurableInbox();
            });
        }
    }
}
