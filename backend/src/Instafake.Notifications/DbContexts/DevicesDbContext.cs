using Instafake.Notifications.DbContexts.Config;
using Instafake.Notifications.Entities;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Notifications.DbContexts;

public class DevicesDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Device> Devices => Set<Device>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new DeviceConfiguration());
    }
}
