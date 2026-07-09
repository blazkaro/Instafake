using Instafake.Profiles.Infrastructure.DbContexts;

namespace Instafake.Profiles.Infrastructure.Services;

public interface IProfileSequenceAllocator
{
    Task<long> Next(string profileId, ProfilesDbContext dbContext);
}
