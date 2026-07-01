using Confluent.Kafka;
using Instafake.Profiles.Application;
using Instafake.Profiles.Application.Repositories;
using Instafake.Profiles.Domain.Entities;
using Instafake.Profiles.Domain.Events;
using Instafake.Profiles.Infrastructure.DbContexts;
using Instafake.Profiles.Infrastructure.Repositories;
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
            services.AddDbContextWithWolverineIntegration<ProfilesDbContext>(cfg =>
            {
                cfg.UseNpgsql(configuration.GetConnectionString("profiles-api-db"))
                    .UseSnakeCaseNamingConvention(); // we let wolverine manage migrations, so we need consistent naming convention between the db context and the migrations
            });

            services.AddScoped<IWriteRepository<Profile>, ProfileWriteRepository>();
            services.AddScoped<IWriteRepository<Follow>, FollowWriteRepository>();

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
                options.UseEntityFrameworkCoreWolverineManagedMigrations();

                options.Policies.AutoApplyTransactions();

                options.UseEntityFrameworkCoreTransactions();

                var kafkaHost = configuration.GetRequiredSection("Kafka:Host").Get<ConsumerConfig>()!;
                options.UseKafka(kafkaHost.BootstrapServers);

                var kafkaDefaultConsumer = configuration.GetSection("Kafka:Consumers:Default").Get<ConsumerConfig>() ?? new ConsumerConfig();

                var usersConsumer = new ConsumerConfig(kafkaDefaultConsumer);
                configuration.GetSection("Kafka:Consumers:Users").Bind(usersConsumer);

                options.LocalQueue("follow-events");

                options.PublishMessage<FollowCreatedEvent>()
                    .ToLocalQueue("follow-events");

                options.PublishMessage<FollowDeletedEvent>()
                    .ToLocalQueue("follow-events");

                options.ListenToKafkaTopic("users")
                    .ConfigureConsumer(cfg =>
                    {
                        foreach (var keyPair in usersConsumer)
                        {
                            cfg.Set(keyPair.Key, keyPair.Value);
                        }
                    }).UseDurableInbox();
            });

            builder.UseResourceSetupOnStartup();
        }
    }
}
