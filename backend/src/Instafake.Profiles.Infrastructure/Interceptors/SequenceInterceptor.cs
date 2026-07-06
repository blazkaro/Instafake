using Instafake.Profiles.Application.Options;
using Instafake.Profiles.Infrastructure.Entities;
using Instafake.Profiles.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace Instafake.Profiles.Infrastructure.Interceptors;

internal class SequenceInterceptor(IProfileSequenceAllocator profileSequenceAllocator, IOptions<FollowBucketOptions> bucketOptions) : SaveChangesInterceptor
{
    private readonly IProfileSequenceAllocator _profileSequenceAllocator = profileSequenceAllocator;
    private readonly FollowBucketOptions _bucketOptions = bucketOptions.Value;

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var entries = eventData.Context.ChangeTracker.Entries<Follow>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                var seq = await _profileSequenceAllocator.Next(entry.Entity.ProfileId);
                entry.Entity.Seq = seq;
                entry.Entity.BucketId = (int)(seq / _bucketOptions.BucketSize);
            }
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
