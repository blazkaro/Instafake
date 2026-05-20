using Instafake.Posts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Posts.Infrastructure.DbContexts.Config;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.
            HasKey(user => user.Id);

        builder
            .HasMany(user => user.AuthoredPosts)
            .WithOne(post => post.Author);

        builder
            .HasMany(user => user.AuthoredComments)
            .WithOne(postComm => postComm.Author)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(user => user.LikedPosts)
            .WithMany(post => post.LikedBy);
    }
}
