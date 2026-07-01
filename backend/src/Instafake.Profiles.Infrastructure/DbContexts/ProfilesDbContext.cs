using Instafake.Profiles.Infrastructure.DbContexts.Config;
using Instafake.Profiles.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Profiles.Infrastructure.DbContexts;

public class ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : DbContext(options)
{
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<Follow> Follows => Set<Follow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new ProfileConfiguration())
            .ApplyConfiguration(new FollowConfiguration());
    }
}
