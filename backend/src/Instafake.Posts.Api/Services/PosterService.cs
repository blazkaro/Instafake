using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Instafake.Posts.Api.Extensions;
using Instafake.Posts.Api.Protos;
using Instafake.Posts.Application.Commands;
using Instafake.Posts.Application.Queries;
using MediatR;

namespace Instafake.Posts.Api.Services;

public class PosterService(IMediator mediator) : Poster.PosterBase
{
    private readonly IMediator _mediator = mediator;

    public override async Task<CreatePostReply> CreatePost(CreatePostRequest request, ServerCallContext context)
    {
        var userId = context.GetAccessTokenSubject()!;
        var postId = await _mediator.Send(new CreatePostCommand(userId, request.Description, [.. request.MultimediaUrls], [.. request.Tags]), context.CancellationToken);
        return new CreatePostReply { Id = postId.ToString() };
    }

    public override async Task<GetCommentsReply> GetComments(GetCommentsRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.PostId, out var postId))
        {
            return new GetCommentsReply(); // empty response, invalid post id
        }

        var commentsPaginated = await _mediator.Send(new GetCommentsQuery(postId, request.Cursor.ToCursorPagination()), context.CancellationToken);
        var reply = new GetCommentsReply()
        {
            Comments = { commentsPaginated.Result.Select(comment => new Protos.CommentDto
            {
                Id = comment.Id,
                Author = new Protos.AuthorDto { Id = comment.Author.Id, Name = comment.Author.Name, AvatarUrl = comment.Author.AvatarUrl },
                Content = comment.Content,
                CreatedAt = comment.CreatedAt.AsUtc().ToTimestamp(),
            }) },
            NextCursor = commentsPaginated.NextCursor.ToProtoCursor()
        };

        return reply;
    }

    public override async Task<GetPostsReply> GetPosts(GetPostsRequest request, ServerCallContext context)
    {
        var userId = context.GetAccessTokenSubject()!;
        var postsPaginated = await _mediator.Send(new GetPostsQuery(userId, request.AuthorId, [.. request.Tags], request.Cursor.ToCursorPagination()), context.CancellationToken);
        var reply = new GetPostsReply()
        {
            Posts = { postsPaginated.Result.Select(post => new Protos.PostDto
            {
                Id = post.Id,
                Author = new Protos.AuthorDto { Id = post.Author.Id, Name = post.Author.Name, AvatarUrl = post.Author.AvatarUrl },
                Description = post.Description,
                MultimediaUrls = { post.MultimediaUrls },
                Tags = { post.Tags },
                CreatedAt = post.CreatedAt.AsUtc().ToTimestamp(),
                LikesCount = post.LikesCount,
                LikedByUser = post.LikedByUser,
                CommentsCount = post.CommentsCount
            }) },
            NextCursor = postsPaginated.NextCursor.ToProtoCursor()
        };

        return reply;
    }
}
