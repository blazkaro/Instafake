using Instafake.Posts.Application.Queries;
using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Application.Queries.Pagination;
using Instafake.Posts.Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Handlers.Queries;

internal class GetPostsQueryHandler(PostsDbContext dbContext) : IRequestHandler<GetPostsQuery, PaginationResult<IReadOnlyList<PostDto>>>
{
    private readonly PostsDbContext _dbContext = dbContext;

    private const int FALLBACK_PAGE_SIZE = 20;

    public async Task<PaginationResult<IReadOnlyList<PostDto>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Posts.AsNoTracking();

        if (!string.IsNullOrEmpty(request.AuthorName))
            query = query.Where(p => p.Author.Name == request.AuthorName);

        if (request.Tags is not null && request.Tags.Length != 0)
            query = query.Where(p => p.Tags.Any(tag => request.Tags.Contains(tag.Tag)));

        if (request.Pagination?.Cursor is not null && Guid.TryParse(request.Pagination.Cursor.Id, out var cursorId))
            query = query.Where(p => p.CreatedAt < request.Pagination.Cursor.LastItemCreatedAt || (p.CreatedAt == request.Pagination.Cursor.LastItemCreatedAt && p.Id < cursorId));

        var pageSize = request.Pagination?.PageSize ?? FALLBACK_PAGE_SIZE;
        var posts = await query
            .OrderByDescending(p => p.CreatedAt)
            .ThenByDescending(p => p.Id)
            .Take(pageSize)
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

        var lastPost = posts.LastOrDefault();
        var nextCursor = lastPost is not null ? new CursorPagination
        {
            Id = lastPost.Id,
            LastItemCreatedAt = lastPost.CreatedAt
        } : null;

        return new()
        {
            Result = posts,
            PageSize = Math.Min(pageSize, posts.Count),
            NextCursor = nextCursor,
        };
    }
}
