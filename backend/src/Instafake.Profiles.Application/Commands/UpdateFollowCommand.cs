namespace Instafake.Profiles.Application.Commands;

public record UpdateFollowCommand(string ProfileId, string FollowerId, bool Follow)
{
}
