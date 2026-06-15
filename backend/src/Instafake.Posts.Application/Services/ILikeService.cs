namespace Instafake.Posts.Application.Services;

public interface ILikeService
{
    Task Like(Guid postId, string userId, CancellationToken cancellationToken = default);
    Task Dislike(Guid postId, string userId, CancellationToken cancellationToken = default);
}
