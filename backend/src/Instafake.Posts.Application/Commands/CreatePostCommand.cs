using MediatR;

namespace Instafake.Posts.Application.Commands;

public record CreatePostCommand(string AuthorId, string Description, string[] MultimediaUrls, string[] Tags) : IRequest<Guid>
{
}
