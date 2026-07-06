using Instafake.Profiles.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Profiles.Infrastructure.DbContexts.Config;

internal class ProfileCounterConfiguration : IEntityTypeConfiguration<ProfileCounter>
{
    public void Configure(EntityTypeBuilder<ProfileCounter> builder)
    {
        builder
            .HasKey(p => p.ProfileId);

        builder
            .HasOne(pc => pc.Profile)
            .WithOne(p => p.ProfileCounter);
    }
}
