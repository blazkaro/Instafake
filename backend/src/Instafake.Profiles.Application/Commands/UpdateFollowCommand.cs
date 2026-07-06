namespace Instafake.Profiles.Application.Commands;

public record UpdateFollowCommand(Guid ProfileId, Guid FollowerId, bool Follow)
{
}
