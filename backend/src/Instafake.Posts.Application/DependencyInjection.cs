using Microsoft.Extensions.DependencyInjection;
using Wolverine;

namespace Instafake.Posts.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices()
        {
            return services;
        }
    }

    extension(WolverineOptions options)
    {
        public WolverineOptions ConfigureApplication()
        {
            options.Discovery.IncludeAssembly(typeof(DependencyInjection).Assembly);
            return options;
        }
    }
}
