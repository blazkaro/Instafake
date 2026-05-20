using Instafake.Posts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Instafake.Posts.Infrastructure.DbContexts.Config;

internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
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
