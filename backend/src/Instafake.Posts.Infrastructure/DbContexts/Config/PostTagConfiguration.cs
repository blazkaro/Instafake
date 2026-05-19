using Instafake.Posts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Posts.Infrastructure.DbContexts.Config;

internal class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
{
    public void Configure(EntityTypeBuilder<PostTag> builder)
    {
        builder
            .HasKey(postTag => postTag.Id);

        builder
            .HasIndex(postTag => postTag.Tag);

        builder
            .HasOne(postTag => postTag.Post)
            .WithMany(post => post.Tags);
    }
}
