namespace Instafake.Posts.Application.Commands;

public record CreatePostCommand(Guid AuthorId, string Description, string[] MultimediaUrls, string[] Tags)
{
}
