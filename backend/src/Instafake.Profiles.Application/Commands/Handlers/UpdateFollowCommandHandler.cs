using FluentResults;
using Instafake.Profiles.Application.Repositories;
using Instafake.Profiles.Domain.Entities;
using Instafake.Profiles.Domain.Events;
using Wolverine;

namespace Instafake.Profiles.Application.Commands.Handlers;

internal class UpdateFollowCommandHandler
{
    public async Task<(Result Result, OutgoingMessages)> Handle(UpdateFollowCommand request, IWriteRepository<Follow> repo, CancellationToken cancellationToken)
    {
        var follow = new Follow { ProfileId = request.ProfileId, FollowerId = request.FollowerId };
        if (request.Follow)
        {
            await repo.InsertAsync(follow, cancellationToken);
            follow.AddEvent(new FollowCreatedEvent(follow.ProfileId, follow.FollowerId));
        }
        else
        {
            await repo.DeleteAsync(follow, cancellationToken);
            follow.AddEvent(new FollowDeletedEvent(follow.ProfileId, follow.FollowerId));
        }

        return (Result.Ok(), new OutgoingMessages(follow.Events));
    }
}
