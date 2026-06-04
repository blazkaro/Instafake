using Instafake.Posts.Application.Events;
using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Events.User;
using Instafake.Posts.Infrastructure.Factories;
using Instafake.Posts.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Instafake.Posts.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services.AddDbContext<PostsDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("Mssql"));
            });

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();

            services.AddSingleton<IConsumerFactory, ConsumerFactory>();

            services.AddKeyedSingleton<IEventsConsumer, KafkaUserEventsConsumer>("authors");

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }
    }
}
