using Instafake.Profiles.Application.Dtos;
using Wolverine.Attributes;

namespace Instafake.Profiles.Infrastructure.Events.Self;

[MessageIdentity("post.notification.fanout.next")]
public record NotificationFanoutNext(Guid PostId, PostAuthorDto Author, int TotalBuckets, int NextBucket, int BatchSize)
{
}
