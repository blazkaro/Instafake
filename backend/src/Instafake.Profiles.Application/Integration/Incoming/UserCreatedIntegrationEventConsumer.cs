using Instafake.Profiles.Application.Repositories;
using Instafake.Profiles.Domain.Entities;
using Instafake.Profiles.Domain.Events;
using Wolverine;

namespace Instafake.Profiles.Application.Integration.Incoming;

public class UserCreatedIntegrationEventConsumer(IWriteRepository<Profile> repo)
{
    private readonly IWriteRepository<Profile> _repo = repo;

    public async Task<OutgoingMessages> Handle(UserCreatedIntegrationEvent ev, CancellationToken cancellationToken)
    {
        var user = new User { Id = ev.UserId, Name = ev.UserName, AvatarUrl = ev.AvatarUrl };
        var profile = new Profile
        {
            User = user,
            PostsCount = 0,
            FollowersCount = 0,
            Description = string.Empty
        };

        await _repo.InsertAsync(profile, cancellationToken);
        profile.AddEvent(new ProfileCreatedEvent(profile.User.Id));

        return new OutgoingMessages(profile.Events);
    }
}
