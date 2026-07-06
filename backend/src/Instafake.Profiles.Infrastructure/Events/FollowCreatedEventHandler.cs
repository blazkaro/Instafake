using Instafake.Profiles.Domain.Events;
using Instafake.Profiles.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Profiles.Infrastructure.Events;

public class FollowCreatedEventHandler
{
    public async Task Handle(FollowCreatedEvent ev, ProfilesDbContext dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Profiles
            .Where(profile => profile.Id == ev.ProfileId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.FollowersCount, p => p.FollowersCount + 1),
            cancellationToken);
    }
}
