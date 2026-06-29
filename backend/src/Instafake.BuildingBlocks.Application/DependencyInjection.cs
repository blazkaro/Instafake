using Instafake.BuildingBlocks.Application.Events.DomainIntegration;
using Microsoft.Extensions.DependencyInjection;

namespace Instafake.BuildingBlocks.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationBuildingBlocks()
        {
            services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();
            return services;
        }
    }
}
