using Instafake.Profiles.Application.Repositories;
using Instafake.Profiles.Domain.Entities;
using Instafake.Profiles.Domain.ValueObjects;

namespace Instafake.Profiles.Application.Events.Incoming.Users;

public class UserCreatedEventConsumer(IWriteRepository<Profile> repo)
{
    private readonly IWriteRepository<Profile> _repo = repo;

    public async Task Handle(UserCreatedEvent ev, CancellationToken cancellationToken)
    {
        var user = new User(ev.UserId, ev.UserName, ev.AvatarUrl);
        var profile = new Profile
        {
            User = user,
            PostsCount = 0,
            FollowersCount = 0,
            Description = string.Empty
        };

        await _repo.InsertAsync(profile, cancellationToken);
    }
}
