using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Instafake.Profiles.Infrastructure.DbContexts;

internal class ProfilesDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProfilesDbContext>
{
    public ProfilesDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
           .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ProfilesDbContext>()
            .UseNpgsql(configuration.GetConnectionString("profiles-api-db"));

        return new ProfilesDbContext(optionsBuilder.Options);
    }
}
