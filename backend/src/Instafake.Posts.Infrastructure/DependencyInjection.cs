using Confluent.Kafka;
using Instafake.Posts.Application;
using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Events;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Repositories;
using JasperFx.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.Kafka;
using Wolverine.SqlServer;

namespace Instafake.Posts.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
        {
            services.AddDbContextWithWolverineIntegration<PostsDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("posts-api-db"));
            });

            services.AddScoped<IWriteRepository<Domain.Entities.Post>, PostWriteRepository>();
            services.AddScoped<IWriteRepository<Domain.Entities.Comment>, CommentWriteRepository>();
            services.AddScoped<IWriteRepository<Domain.Entities.Author>, AuthorWriteRepository>();
            services.AddScoped<IWriteRepository<Domain.Entities.PostLike>, PostLikeWriteRepository>();

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

                options.PersistMessagesWithSqlServer(configuration.GetConnectionString("posts-api-db"));
                options.UseEntityFrameworkCoreWolverineManagedMigrations();

                options.Policies.AutoApplyTransactions();

                options.UseEntityFrameworkCoreTransactions();

                var kafkaHost = configuration.GetRequiredSection("Kafka:Host").Get<ConsumerConfig>()!;
                options.UseKafka(kafkaHost.BootstrapServers)
                    .AutoProvision();

                var kafkaDefaultConsumer = configuration.GetSection("Kafka:Consumers:Default").Get<ConsumerConfig>() ?? new ConsumerConfig();

                var usersConsumer = new ConsumerConfig(kafkaDefaultConsumer);
                configuration.GetSection("Kafka:Consumers:Users").Bind(usersConsumer);

                options.LocalQueue("comment-events");
                options.LocalQueue("post-like-events");

                options.PublishMessage<CommentCreatedEvent>()
                    .ToLocalQueue("comment-events");

                options.PublishMessage<PostLikeCreatedEvent>()
                    .ToLocalQueue("post-like-events");

                options.PublishMessage<PostLikeDeletedEvent>()
                    .ToLocalQueue("post-like-events");

                options.ListenToKafkaTopic("users")
                    .ConfigureConsumer(cfg =>
                    {
                        foreach (var keyPair in usersConsumer)
                        {
                            cfg.Set(keyPair.Key, keyPair.Value);
                        }
                    }).UseDurableInbox();

                options.PublishMessage<PostCreatedEvent>()
                    .ToKafkaTopic("post-events")
                    .UseDurableOutbox();
            });

            builder.UseResourceSetupOnStartup();
        }
    }
}
