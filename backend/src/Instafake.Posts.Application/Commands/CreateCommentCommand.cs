namespace Instafake.Posts.Application.Commands;

public record CreateCommentCommand(Guid AuthorId, Guid PostId, string Content)
{
}
