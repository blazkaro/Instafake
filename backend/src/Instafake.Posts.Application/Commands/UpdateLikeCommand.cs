namespace Instafake.Posts.Application.Commands;

public record UpdateLikeCommand(bool Like, Guid PostId, string UserId)
{
}
