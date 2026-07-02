using FluentResults;
using Instafake.Posts.Application.Queries;
using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Application.Queries.Pagination;
using Instafake.Posts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Instafake.Posts.Infrastructure.QueryHandlers;

public class GetPostsQueryHandler
{
    private const int FALLBACK_PAGE_SIZE = 20;

    [NonTransactional]
    public async Task<Result<PaginationResult<PostDto>>> Handle(GetPostsQuery request, PostsDbContext dbContext, CancellationToken cancellationToken)
    {
        var query = dbContext.Posts.AsNoTracking();

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
            .Select(post => new PostDto
            (
                post.Id.ToString(),
                new AuthorDto(post.AuthorId, post.Author.Name, post.Author.AvatarUrl),
                post.Description,
                post.MultimediaUrls,
                post.Tags.Select(tag => tag.Tag).ToList(),
                post.CreatedAt,
                post.LikesCount,
                post.Likes.Any(p => p.UserId == request.UserId),
                post.CommentsCount
            ))
            .ToListAsync(cancellationToken);

        var lastPost = posts.LastOrDefault();
        var nextCursor = lastPost is not null ? new CursorPagination
        {
            Id = lastPost.Id,
            LastItemCreatedAt = lastPost.CreatedAt
        } : null;

        return Result.Ok(new PaginationResult<PostDto>
        {
            Items = posts,
            PageSize = Math.Min(pageSize, posts.Count),
            NextCursor = nextCursor,
        });
    }
}
