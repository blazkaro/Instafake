using Instafake.Posts.Application.Events;
using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Application.Services;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Events.User;
using Instafake.Posts.Infrastructure.Factories;
using Instafake.Posts.Infrastructure.Repositories;
using Instafake.Posts.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Instafake.Posts.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructureServices(IConfiguration configuration)
        {
            services.AddDbContext<PostsDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString("Mssql"));
            });

            services.AddScoped<IWriteRepository<Domain.Entities.Post>, PostWriteRepository>();
            services.AddScoped<IWriteRepository<Domain.Entities.Comment>, CommentWriteRepository>();
            services.AddScoped<IWriteRepository<Domain.Entities.Author>, AuthorWriteRepository>();

            services.AddSingleton<IConsumerFactory, ConsumerFactory>();
            services.AddKeyedSingleton<IEventsConsumer, KafkaUserEventsConsumer>("authors");

            services.AddScoped<ILikeService, LikeService>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }
    }
}
