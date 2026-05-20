using Instafake.Posts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Posts.Infrastructure.DbContexts.Config;

internal class CommentConfiguration : IEntityTypeConfiguration<PostComment>
{
    public void Configure(EntityTypeBuilder<PostComment> builder)
    {
        builder
            .HasKey(postComm => postComm.Id);

        builder
            .HasIndex(postComm => postComm.PostId);

        builder
            .HasOne(postComm => postComm.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(postComm => postComm.PostId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(postComm => postComm.Author)
            .WithMany(post => post.AuthoredComments)
            .HasForeignKey(postComm => postComm.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
