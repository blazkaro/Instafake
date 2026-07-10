using Instafake.Profiles.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Instafake.Profiles.Infrastructure.Services;

public class ProfileSequenceAllocator : IProfileSequenceAllocator
{
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

    public async Task<long> Next(string profileId, ProfilesDbContext dbContext)
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
            var nextSeq = await dbContext.Database
                .SqlQuery<long>($@"
                    UPDATE ""ProfileCounters""
                    SET ""NextSeq"" = ""NextSeq"" + {BLOCK_SIZE}
                    WHERE ""ProfileId"" = {profileId}
                    RETURNING ""NextSeq""")
                .AsAsyncEnumerable()
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
