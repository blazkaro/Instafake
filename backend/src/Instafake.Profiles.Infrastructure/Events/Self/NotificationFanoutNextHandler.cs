using Instafake.Profiles.Application.Integration.Outgoing;
using Instafake.Profiles.Application.Options;
using Instafake.Profiles.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Wolverine;
using Wolverine.Attributes;

namespace Instafake.Profiles.Infrastructure.Events.Self;

public class NotificationFanoutNextHandler
{
    // TODO: Currently, to pass next fanout message to the Kafka, we are waiting till we create (not emit) all post.notification events.
    // It takes time because we need to query follower ids from db
    // Passing a fanout message and actually processing it should be done by two different handlers.
    [NonTransactional]
    public async Task<OutgoingMessages> Handle(NotificationFanoutNext fanoutNext, ProfilesDbContext dbContext, IOptions<FollowBucketOptions> bucketOptions, CancellationToken cancellationToken)
    {
        var messages = new OutgoingMessages();

        var nextBucket = Math.Min(fanoutNext.NextBucket + fanoutNext.BatchSize, fanoutNext.TotalBuckets);
        if (nextBucket < fanoutNext.TotalBuckets)
        {
            var fanoutContinuation = fanoutNext with { NextBucket = nextBucket };
            messages.Add(fanoutContinuation);
        }

        var bucketFollower = await dbContext.Follows
            .AsNoTracking()
            .Where(p => p.ProfileId == fanoutNext.Author.Id && p.BucketId >= fanoutNext.NextBucket && p.BucketId < nextBucket)
            .Select(p => new { p.BucketId, p.FollowerId })
            .ToListAsync(cancellationToken);

        var buckets = bucketFollower
            .GroupBy(p => p.BucketId)
            .ToDictionary(k => k.Key, v => v.Select(p => p.FollowerId).ToList());

        for (var bucketId = fanoutNext.NextBucket; bucketId < nextBucket; bucketId++)
        {
            if (buckets.TryGetValue(bucketId, out var followerIds))
            {
                messages.Add(new PostNotification(fanoutNext.PostId, fanoutNext.Author, followerIds));
            }
        }

        return messages;
    }
}
