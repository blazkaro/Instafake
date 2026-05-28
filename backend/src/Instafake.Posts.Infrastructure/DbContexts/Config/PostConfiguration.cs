using Instafake.Posts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Posts.Infrastructure.DbContexts.Config;

internal class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder
            .HasKey(post => post.Id);

        builder
            .HasIndex(post => post.CreatedAt);

        builder
            .HasOne(post => post.Author)
            .WithMany(user => user.AuthoredPosts)
            .HasForeignKey(post => post.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(post => post.LikedBy)
            .WithMany(user => user.LikedPosts);

        builder
            .HasMany(post => post.Tags)
            .WithOne(postTag => postTag.Post);
    }
}
