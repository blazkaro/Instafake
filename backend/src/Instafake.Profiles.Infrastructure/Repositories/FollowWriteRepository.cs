using Instafake.Profiles.Application.Repositories;
using Instafake.Profiles.Domain.Entities;
using Instafake.Profiles.Infrastructure.DbContexts;
using Instafake.Profiles.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Profiles.Infrastructure.Repositories;

internal class FollowWriteRepository(ProfilesDbContext dbContext) : IWriteRepository<Follow>
{
    private readonly ProfilesDbContext _dbContext = dbContext;

    public async Task DeleteAsync(Follow entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Follows
           .Where(follow => follow.ProfileId == entity.ProfileId && follow.FollowerId == entity.FollowerId)
           .ExecuteDeleteAsync(cancellationToken);
    }

    public Task InsertAsync(Follow entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Follows.Add(entity.ToEntity());
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Follow entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
