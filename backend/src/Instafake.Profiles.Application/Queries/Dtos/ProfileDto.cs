namespace Instafake.Profiles.Application.Queries.Dtos;

public record ProfileDto(string UserId, string UserName, string AvatarUrl, int PostsCount, int FollowersCount, string Description, bool FollowedByUser)
{
}
