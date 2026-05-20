using Instafake.Posts.Domain.Entities;

namespace Instafake.Posts.Application.Repositories;

public interface IPostRepository
{
    Task<Post> SaveAsync(Post post, CancellationToken cancellationToken = default);
    Task<Post> UpdateAsync(Post post, CancellationToken cancellationToken = default);
}
