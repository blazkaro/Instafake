using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Infrastructure.DbContexts;
using Instafake.Posts.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Instafake.Posts.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            services.AddDbContext<PostsDbContext>();

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            return services;
        }
    }
}
