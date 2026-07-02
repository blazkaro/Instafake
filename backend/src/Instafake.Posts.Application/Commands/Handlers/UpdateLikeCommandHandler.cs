using FluentResults;
using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Domain.Events;
using Wolverine;

namespace Instafake.Posts.Application.Commands.Handlers;

public class UpdateLikeCommandHandler
{
    public async Task<(Result Result, OutgoingMessages)> Handle(UpdateLikeCommand request, IWriteRepository<PostLike> repo, CancellationToken cancellationToken)
    {
        var like = new PostLike { UserId = request.UserId, PostId = request.PostId };
        if (request.Like)
        {
            await repo.InsertAsync(like, cancellationToken);
            like.AddEvent(new PostLikeCreatedEvent(like.PostId, like.UserId));
        }
        else
        {
            await repo.DeleteAsync(like, cancellationToken);
            like.AddEvent(new PostLikeDeletedEvent(like.PostId, like.UserId));
        }

        return (Result.Ok(), new OutgoingMessages(like.Events));
    }
}
