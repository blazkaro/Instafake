using Instafake.Posts.Application.Commands;
using Instafake.Posts.Application.Repositories;
using MediatR;

namespace Instafake.Posts.Infrastructure.Handlers.Commands;

internal class CreateCommentCommandHandler(ICommentRepository repo) : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly ICommentRepository _repo = repo;

    public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new Domain.Entities.Comment
        {
            Id = Guid.NewGuid(),
            AuthorId = request.AuthorId,
            PostId = request.PostId,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.SaveAsync(comment, cancellationToken);
        return comment.Id;
    }
}
