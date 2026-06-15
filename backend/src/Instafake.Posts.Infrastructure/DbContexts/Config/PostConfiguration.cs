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
            .HasIndex(post => new { post.CreatedAt, post.Id })
            .IsUnique()
            .IsDescending(true, true);

        builder
            .HasOne(post => post.Author)
            .WithMany(user => user.AuthoredPosts)
            .HasForeignKey(post => post.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(post => post.Likes)
            .WithOne(like => like.Post)
            .HasForeignKey(like => like.PostId);

        builder
            .HasMany(post => post.Tags)
            .WithOne(postTag => postTag.Post);
    }
}
