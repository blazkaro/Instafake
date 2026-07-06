using Instafake.Profiles.Domain.Events;
using Instafake.Profiles.Infrastructure.DbContexts;
using Instafake.Profiles.Infrastructure.Entities;

namespace Instafake.Profiles.Infrastructure.Events;

public class ProfileCreatedEventHandler
{
    public async Task Handle(ProfileCreatedEvent ev, ProfilesDbContext dbContext, CancellationToken cancellationToken)
    {
        var profileCounter = new ProfileCounter()
        {
            ProfileId = ev.ProfileId,
            NextSeq = 0
        };

        dbContext.ProfileCounters.Add(profileCounter);
    }
}
