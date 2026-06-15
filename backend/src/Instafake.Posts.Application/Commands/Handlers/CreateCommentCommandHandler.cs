using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using MediatR;

namespace Instafake.Posts.Application.Commands.Handlers;

internal class CreateCommentCommandHandler(IWriteRepository<Comment> repo) : IRequestHandler<CreateCommentCommand, Guid>
{
    private readonly IWriteRepository<Comment> _repo = repo;

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
