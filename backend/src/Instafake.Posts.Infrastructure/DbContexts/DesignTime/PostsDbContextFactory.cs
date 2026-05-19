using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Instafake.Posts.Infrastructure.DbContexts.DesignTime;

internal class PostsDbContextFactory : IDesignTimeDbContextFactory<PostsDbContext>
{
    public PostsDbContext CreateDbContext(string[] args)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddUserSecrets<PostsDbContextFactory>(false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PostsDbContext>();
        optionsBuilder.UseSqlServer(configBuilder.GetConnectionString("Mssql"));

        return new PostsDbContext(optionsBuilder.Options);
    }
}
