using Instafake.Profiles.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Instafake.Profiles.Infrastructure.Services;

public class ProfileSequenceAllocator(IDbContextFactory<ProfilesDbContext> dbContextFactory) : IProfileSequenceAllocator
{
    private readonly IDbContextFactory<ProfilesDbContext> _dbContextFactory = dbContextFactory;

    private class LeaseBlock
    {
        public long StartSeq { get; set; }
        public long NextSeq { get; set; }
        public long EndSeq { get; set; }

        public SemaphoreSlim Lock { get; } = new SemaphoreSlim(1, 1);
    }

    // TODO: cleanup for not used profiles
    private readonly ConcurrentDictionary<string, LeaseBlock> _profileLeaseBlocks = new();
    private const int BLOCK_SIZE = 1000;

    public async Task<long> Next(string profileId)
    {
        var lease = _profileLeaseBlocks.GetOrAdd(profileId, _ => new LeaseBlock());

        await lease.Lock.WaitAsync();
        try
        {
            if (lease.NextSeq <= lease.EndSeq)
            {
                var curSeq = lease.NextSeq;
                lease.NextSeq += 1;
                return curSeq;
            }

            // We trust that the counter was inserted earlier when the profile was created
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            var nextSeq = await dbContext.Database
                .SqlQuery<long>($@"
                    UPDATE profile_counters
                    SET next_seq = next_seq + {BLOCK_SIZE}
                    WHERE profile_id = {profileId}
                    RETURNING next_seq")
                .FirstAsync();

            lease.EndSeq = nextSeq - 1;
            lease.StartSeq = nextSeq - BLOCK_SIZE;
            lease.NextSeq = lease.StartSeq + 1;

            return lease.StartSeq;
        }
        finally
        {
            lease.Lock.Release();
        }
    }
}
