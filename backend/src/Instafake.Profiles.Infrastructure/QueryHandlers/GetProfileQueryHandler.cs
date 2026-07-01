using FluentResults;
using Instafake.Profiles.Application.Errors;
using Instafake.Profiles.Application.Queries;
using Instafake.Profiles.Application.Queries.Dtos;
using Instafake.Profiles.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Instafake.Profiles.Infrastructure.QueryHandlers;

internal class GetProfileQueryHandler
{
    [NonTransactional]
    public async Task<Result<ProfileDto>> Handle(GetProfileQuery request, ProfilesDbContext dbContext, CancellationToken cancellationToken)
    {
        var profile = await dbContext.Profiles
            .AsNoTracking()
            .Where(profile => profile.Name == request.UserName)
            .SingleOrDefaultAsync(cancellationToken);

        if (profile == default)
            return Result.Fail(new ResourceNotFound());

        var dto = new ProfileDto(profile.Id, profile.Name, profile.AvatarUrl, profile.PostsCount, profile.FollowersCount, profile.Description);
        return Result.Ok(dto);
    }
}
