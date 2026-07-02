using FluentResults;
using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Domain.Events;
using Wolverine;

namespace Instafake.Posts.Application.Commands.Handlers;

public class CreateCommentCommandHandler
{
    public async Task<(Result<Guid> Result, OutgoingMessages)> Handle(CreateCommentCommand request, IWriteRepository<Comment> repo, CancellationToken cancellationToken)
    {
        var comment = new Domain.Entities.Comment
        {
            Id = Guid.NewGuid(),
            AuthorId = request.AuthorId,
            PostId = request.PostId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        await repo.InsertAsync(comment, cancellationToken);
        comment.AddEvent(new CommentCreatedEvent(comment.PostId, comment.Id));

        return (Result.Ok(comment.Id), new OutgoingMessages(comment.Events));
    }
}
