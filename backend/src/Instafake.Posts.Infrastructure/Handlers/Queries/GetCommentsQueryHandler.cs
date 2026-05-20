using Instafake.Posts.Application.Queries;
using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Infrastructure.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Instafake.Posts.Infrastructure.Handlers.Queries;

internal class GetCommentsQueryHandler(PostsDbContext dbContext) : IRequestHandler<GetCommentsQuery, IReadOnlyList<CommentDto>>
{
    private readonly PostsDbContext _dbContext = dbContext;

    const int FALLBACK_PAGE_SIZE = 50;

    public async Task<IReadOnlyList<CommentDto>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Comments.AsNoTracking()
            .Where(p => p.PostId == request.PostId);

        if (request.Cursor is not null && Guid.TryParse(request.Cursor.Id, out var cursorId))
            query = query.Where(p => p.CreatedAt < request.Cursor.LastItemCreatedAt || (p.CreatedAt == request.Cursor.LastItemCreatedAt && p.Id < cursorId));

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ThenByDescending(p => p.Id)
            .Take(request.Cursor?.PageSize ?? FALLBACK_PAGE_SIZE)
            .Select(p => new CommentDto
            (
                p.Id.ToString(),
                new AuthorDto(p.AuthorId, p.Author.Name, p.Author.AvatarUrl),
                p.Content,
                p.CreatedAt
            ))
            .ToListAsync(cancellationToken);
    }
}
