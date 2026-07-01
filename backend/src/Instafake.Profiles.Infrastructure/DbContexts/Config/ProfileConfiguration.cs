using Instafake.Profiles.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Profiles.Infrastructure.DbContexts.Config;

internal class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .HasIndex(p => p.Name)
            .IsUnique();

        builder
            .HasMany(profile => profile.Follows)
            .WithOne(follow => follow.Profile);
    }
}
