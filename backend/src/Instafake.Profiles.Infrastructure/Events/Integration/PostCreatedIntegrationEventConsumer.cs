using Instafake.Profiles.Application.Dtos;
using Instafake.Profiles.Application.Integration.Incoming;
using Instafake.Profiles.Application.Options;
using Instafake.Profiles.Infrastructure.DbContexts;
using Instafake.Profiles.Infrastructure.Events.Self;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wolverine;
using Wolverine.Attributes;

namespace Instafake.Profiles.Infrastructure.Events.Integration;

public class PostCreatedIntegrationEventConsumer
{
    [NonTransactional]
    public async Task<OutgoingMessages> Handle(PostCreatedIntegrationEvent ev, IOptions<FollowBucketOptions> bucketOptions, ProfilesDbContext dbContext, CancellationToken cancellationToken)
    {
        var authorDto = await dbContext.Profiles
            .AsNoTracking()
            .Where(p => p.Id == ev.AuthorId)
            .Select(p => new PostAuthorDto(p.Id, p.Name, new Uri(p.AvatarUrl)))
            .SingleOrDefaultAsync(cancellationToken);

        if (authorDto == default)
            return [];

        var maxSeq = await dbContext.ProfileCounters
            .AsNoTracking()
            .Where(p => p.ProfileId == authorDto.Id)
            .Select(p => p.NextSeq)
            .SingleOrDefaultAsync(cancellationToken);

        int bucketSize = bucketOptions.Value.BucketSize;
        int totalBuckets = Math.Max(1, (int)Math.Ceiling((double)maxSeq / bucketSize)); // maxSeq is first free seq, so we don't add 1 (but take care of maxSeq=0)
        var fanoutNext = new NotificationFanoutNext(ev.PostId, authorDto, totalBuckets, 0, 500);

        return [fanoutNext];
    }

    public async Task Handle(PostCreatedIntegrationEvent ev, ProfilesDbContext dbContext, CancellationToken cancellationToken)
    {
        await dbContext.Profiles
            .Where(profile => profile.Id == ev.AuthorId)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(p => p.PostsCount, p => p.PostsCount + 1),
            cancellationToken);
    }
}
