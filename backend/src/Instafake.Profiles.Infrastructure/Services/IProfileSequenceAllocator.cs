namespace Instafake.Profiles.Infrastructure.Services;

public interface IProfileSequenceAllocator
{
    Task<long> Next(string profileId);
}
