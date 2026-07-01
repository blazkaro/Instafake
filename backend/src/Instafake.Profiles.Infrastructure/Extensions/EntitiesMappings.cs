namespace Instafake.Profiles.Infrastructure.Extensions;

internal static class EntitiesMappings
{
    extension(Domain.Entities.Profile profile)
    {
        public Infrastructure.Entities.Profile ToEntity()
        {
            return new()
            {
                Id = profile.User.Id,
                Name = profile.User.Name,
                AvatarUrl = profile.User.AvatarUrl.ToString(),
                PostsCount = profile.PostsCount,
                FollowersCount = profile.FollowersCount,
                Description = profile.Description
            };
        }
    }

    extension(Domain.Entities.Follow follow)
    {
        public Infrastructure.Entities.Follow ToEntity()
        {
            return new()
            {
                ProfileId = follow.ProfileId,
                FollowerId = follow.FollowerId
            };
        }
    }
}
