using Instafake.Posts.Application.Commands;
using Instafake.Posts.Application.Repositories;
using MediatR;

namespace Instafake.Posts.Infrastructure.Handlers.Commands;

internal class CreatePostCommandHandler(IPostRepository repo) : IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IPostRepository _repo = repo;

    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var post = new Domain.Entities.Post()
        {
            Id = Guid.NewGuid(),
            AuthorId = request.AuthorId,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };

        post.AddMultimedia(request.MultimediaUrls);
        post.AddTag(request.Tags);

        await _repo.SaveAsync(post, cancellationToken);
        return post.Id;
    }
}
