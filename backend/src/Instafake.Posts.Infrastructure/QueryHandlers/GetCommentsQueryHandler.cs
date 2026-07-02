using FluentResults;
using Instafake.Posts.Application.Queries;
using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Application.Queries.Pagination;
using Instafake.Posts.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Instafake.Posts.Infrastructure.QueryHandlers;

public class GetCommentsQueryHandler
{
    const int FALLBACK_PAGE_SIZE = 50;

    [NonTransactional]
    public async Task<Result<PaginationResult<CommentDto>>> Handle(GetCommentsQuery request, PostsDbContext dbContext, CancellationToken cancellationToken)
    {
        var query = dbContext.Comments.AsNoTracking()
            .Where(p => p.PostId == request.PostId);

        if (request.Pagination?.Cursor is not null && Guid.TryParse(request.Pagination.Cursor.Id, out var cursorId))
            query = query.Where(p => p.CreatedAt < request.Pagination.Cursor.LastItemCreatedAt || (p.CreatedAt == request.Pagination.Cursor.LastItemCreatedAt && p.Id < cursorId));

        var pageSize = request.Pagination?.PageSize ?? FALLBACK_PAGE_SIZE;
        var comments = await query
            .OrderByDescending(p => p.CreatedAt)
            .ThenByDescending(p => p.Id)
            .Take(pageSize)
            .Select(p => new CommentDto
            (
                p.Id.ToString(),
                new AuthorDto(p.AuthorId, p.Author.Name, p.Author.AvatarUrl),
                p.Content,
                p.CreatedAt
            ))
            .ToListAsync(cancellationToken);

        var lastComment = comments.LastOrDefault();
        var nextCursor = lastComment is not null ? new CursorPagination
        {
            Id = lastComment.Id,
            LastItemCreatedAt = lastComment.CreatedAt
        } : null;

        return Result.Ok(new PaginationResult<CommentDto>
        {
            Items = comments,
            PageSize = Math.Min(pageSize, comments.Count),
            NextCursor = nextCursor
        });
    }
}
