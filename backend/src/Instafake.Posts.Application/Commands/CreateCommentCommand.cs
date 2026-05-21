using MediatR;

namespace Instafake.Posts.Application.Commands;

public record CreateCommentCommand(string AuthorId, Guid PostId, string Content) : IRequest<Guid>
{
}
