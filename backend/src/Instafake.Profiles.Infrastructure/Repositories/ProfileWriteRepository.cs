using Instafake.Profiles.Application.Repositories;
using Instafake.Profiles.Domain.Entities;
using Instafake.Profiles.Infrastructure.DbContexts;
using Instafake.Profiles.Infrastructure.Extensions;

namespace Instafake.Profiles.Infrastructure.Repositories;

internal class ProfileWriteRepository(ProfilesDbContext dbContext) : IWriteRepository<Profile>
{
    private readonly ProfilesDbContext _dbContext = dbContext;

    public Task DeleteAsync(Profile entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task InsertAsync(Profile entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Profiles.Add(entity.ToEntity());
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Profile entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
