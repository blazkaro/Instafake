using Instafake.Profiles.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Profiles.Infrastructure.DbContexts.Config;

internal class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder
            .HasKey(follow => new { follow.ProfileId, follow.BucketId, follow.Seq });

        builder
            .HasIndex(follow => new { follow.ProfileId, follow.FollowerId })
            .IsUnique();

        builder
            .HasOne(follow => follow.Profile)
            .WithMany(profile => profile.Follows);
    }
}
