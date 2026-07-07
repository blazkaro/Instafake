namespace Instafake.Profiles.Infrastructure.Entities;

public class ProfileCounter
{
    public Profile Profile { get; set; }
    public string ProfileId { get; set; }
    public long NextSeq { get; set; }
}
