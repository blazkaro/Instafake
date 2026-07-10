using Instafake.Notifications.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Notifications.DbContexts.Config;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder
            .HasKey(p => new { p.UserId, p.DeviceToken });

        builder
            .HasIndex(p => p.UserId);
    }
}
