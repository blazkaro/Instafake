using FluentResults;
using Instafake.Profiles.Application.Errors;
using Instafake.Profiles.Application.Queries;
using Instafake.Profiles.Application.Queries.Dtos;
using Instafake.Profiles.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Instafake.Profiles.Infrastructure.QueryHandlers;

public class GetProfileQueryHandler
{
    [NonTransactional]
    public async Task<Result<ProfileDto>> Handle(GetProfileQuery request, ProfilesDbContext dbContext, CancellationToken cancellationToken)
    {
        var dto = await dbContext.Profiles
            .AsNoTracking()
            .Where(profile => profile.Name == request.ProfileName)
            .Select(p => new ProfileDto(
                p.Id,
                p.Name,
                p.AvatarUrl,
                p.PostsCount,
                p.FollowersCount,
                p.Description,
                p.Follows.Any(f => f.FollowerId == request.UserId))
            )
            .SingleOrDefaultAsync(cancellationToken);

        if (dto == default)
            return Result.Fail(new ResourceNotFound());

        return Result.Ok(dto);
    }
}
