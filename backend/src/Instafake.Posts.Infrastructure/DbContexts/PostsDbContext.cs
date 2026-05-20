using Instafake.Posts.Infrastructure.DbContexts.Config;
using Instafake.Posts.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.DbContexts;

internal class PostsDbContext(DbContextOptions<PostsDbContext> options) : DbContext(options)
{
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostComment> Comments => Set<PostComment>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new PostConfiguration())
            .ApplyConfiguration(new CommentConfiguration())
            .ApplyConfiguration(new UserConfiguration())
            .ApplyConfiguration(new PostTagConfiguration());
    }
}
