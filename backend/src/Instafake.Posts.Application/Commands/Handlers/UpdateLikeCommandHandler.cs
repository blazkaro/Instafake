using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Domain.Events;
using MediatR;

namespace Instafake.Posts.Application.Commands.Handlers;

internal class UpdateLikeCommandHandler(IWriteRepository<PostLike> repo, IPublisher publisher) : CommandHandlerBase<PostLike>(publisher), IRequestHandler<UpdateLikeCommand, bool>
{
    private readonly IWriteRepository<PostLike> _repo = repo;

    public async Task<bool> Handle(UpdateLikeCommand request, CancellationToken cancellationToken)
    {
        var like = new PostLike { UserId = request.UserId, PostId = request.PostId };
        try
        {
            if (request.Like)
            {
                await _repo.SaveAsync(like, cancellationToken);
                like.AddEvent(new PostLikeCreatedEvent(like.PostId, like.UserId));
            }
            else
            {
                await _repo.DeleteAsync(like, cancellationToken);
                like.AddEvent(new PostLikeDeletedEvent(like.PostId, like.UserId));
            }
        }
        catch
        {
            await DispatchEvents(like);
            return false;
        }

        await DispatchEvents(like);
        return true;
    }
}
