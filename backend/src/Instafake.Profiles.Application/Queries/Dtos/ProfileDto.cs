namespace Instafake.Profiles.Application.Queries.Dtos;

public record ProfileDto(Guid UserId, string UserName, string AvatarUrl, int PostsCount, int FollowersCount, string Description)
{
}
