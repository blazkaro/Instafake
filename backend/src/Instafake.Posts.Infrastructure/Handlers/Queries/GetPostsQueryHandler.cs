using Instafake.Posts.Application.Queries;
using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Handlers.Queries;

internal class GetPostsQueryHandler(PostsDbContext dbContext) : IRequestHandler<GetPostsQuery, IReadOnlyList<PostDto>>
{
    private readonly PostsDbContext _dbContext = dbContext;

    private const int FALLBACK_PAGE_SIZE = 20;

    public async Task<IReadOnlyList<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Posts.AsNoTracking();

        if (string.IsNullOrEmpty(request.AuthorId))
            query = query.Where(p => p.AuthorId == request.AuthorId);

        if (request.Tags is not null && request.Tags.Length != 0)
            query = query.Where(p => p.Tags.Any(tag => request.Tags.Contains(tag.Tag)));

        if (request.Cursor is not null && Guid.TryParse(request.Cursor.Id, out var cursorId))
            query = query.Where(p => p.CreatedAt < request.Cursor.LastItemCreatedAt || (p.CreatedAt == request.Cursor.LastItemCreatedAt && p.Id < cursorId));

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ThenByDescending(p => p.Id)
            .Take(request.Cursor?.PageSize ?? FALLBACK_PAGE_SIZE)
            .Select(p => new PostDto
            (
                p.Id.ToString(),
                new AuthorDto(p.AuthorId, p.Author.Name, p.Author.AvatarUrl),
                p.Description,
                p.MultimediaUrls,
                p.Tags.Select(tag => tag.Tag).ToList(),
                p.CreatedAt,
                p.LikedBy.Count,
                p.LikedBy.Any(p => p.Id == request.UserId),
                p.Comments.Count
            ))
            .ToListAsync(cancellationToken);
    }
}
