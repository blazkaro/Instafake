using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using MediatR;

namespace Instafake.Posts.Application.Commands.Handlers;

internal class CreatePostCommandHandler(IWriteRepository<Post> repo, IPublisher publisher) : CommandHandlerBase<Post>(publisher), IRequestHandler<CreatePostCommand, Guid>
{
    private readonly IWriteRepository<Post> _repo = repo;

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
