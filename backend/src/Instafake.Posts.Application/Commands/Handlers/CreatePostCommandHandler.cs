using FluentResults;
using Instafake.Posts.Application.Repositories;
using Instafake.Posts.Domain.Entities;
using Instafake.Posts.Domain.Events;
using Wolverine;

namespace Instafake.Posts.Application.Commands.Handlers;

public class CreatePostCommandHandler
{
    public async Task<(Result<Guid> Result, OutgoingMessages)> Handle(CreatePostCommand request, IWriteRepository<Post> repo, CancellationToken cancellationToken)
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

        await repo.InsertAsync(post, cancellationToken);
        post.AddEvent(new PostCreatedEvent(post.Id, post.AuthorId));

        return (Result.Ok(post.Id), new OutgoingMessages(post.Events));
    }
}
