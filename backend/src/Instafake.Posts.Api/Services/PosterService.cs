using FluentResults;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Instafake.Posts.Api.Extensions;
using Instafake.Posts.Api.Protos;
using Instafake.Posts.Application.Commands;
using Instafake.Posts.Application.Queries;
using Instafake.Posts.Application.Queries.Dtos;
using Instafake.Posts.Application.Queries.Pagination;
using Wolverine;

namespace Instafake.Posts.Api.Services;

public class PosterService(IMessageBus bus) : Poster.PosterBase
{
    private readonly IMessageBus _bus = bus;

    public override async Task<CreateCommentReply> CreateComment(CreateCommentRequest request, ServerCallContext context)
    {
        var userId = context.GetAccessTokenSubject()!.Value;
        if (!Guid.TryParse(request.PostId, out var postId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Invalid post id"));
        }

        var result = await _bus.InvokeAsync<Result<Guid>>(new CreateCommentCommand(userId, postId, request.Content), context.CancellationToken);
        if (result.IsFailed)
            throw new RpcException(result.ToGrpcStatus());

        return new CreateCommentReply { Id = result.Value.ToString() };
    }

    public override async Task<CreatePostReply> CreatePost(CreatePostRequest request, ServerCallContext context)
    {
        var userId = context.GetAccessTokenSubject()!.Value;
        var result = await _bus.InvokeAsync<Result<Guid>>(new CreatePostCommand(userId, request.Description, [.. request.MultimediaUrls], [.. request.Tags]), context.CancellationToken);
        if (result.IsFailed)
            throw new RpcException(result.ToGrpcStatus());

        return new CreatePostReply { Id = result.Value.ToString() };
    }

    public override async Task<GetCommentsReply> GetComments(GetCommentsRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.PostId, out var postId))
        {
            return new GetCommentsReply(); // empty response, invalid post id format so surely no data
        }

        var result = await _bus.InvokeAsync<Result<PaginationResult<Application.Queries.Dtos.CommentDto>>>(
            new GetCommentsQuery(postId, request.Pagination.ToPaginationDto()),
            context.CancellationToken);

        if (result.IsFailed)
            throw new RpcException(result.ToGrpcStatus());

        var paginatedResult = result.Value;
        var reply = new GetCommentsReply()
        {
            Items = { paginatedResult.Items.Select(comment => new Protos.CommentDto
            {
                Id = comment.Id,
                Author = new Protos.Shared.AuthorDto { Id = comment.Author.Id.ToString(), Name = comment.Author.Name, AvatarUrl = comment.Author.AvatarUrl },
                Content = comment.Content,
                CreatedAt = comment.CreatedAt.AsUtc().ToTimestamp()
            }) },
            Pagination = new PaginationDto { PageSize = paginatedResult.PageSize, Cursor = paginatedResult.NextCursor }.ToProtoPaginationReply()
        };

        return reply;
    }

    public override async Task<GetPostsReply> GetPosts(GetPostsRequest request, ServerCallContext context)
    {
        var userId = context.GetAccessTokenSubject()!.Value;
        var result = await _bus.InvokeAsync<Result<PaginationResult<Application.Queries.Dtos.PostDto>>>(
            new GetPostsQuery(userId, request.AuthorName, [.. request.Tags], request.Pagination.ToPaginationDto()),
            context.CancellationToken);

        if (result.IsFailed)
            throw new RpcException(result.ToGrpcStatus());

        var postsPaginated = result.Value;
        var reply = new GetPostsReply()
        {
            Items = { postsPaginated.Items.Select(post => new Protos.PostDto
            {
                Id = post.Id,
                Author = new Protos.Shared.AuthorDto { Id = post.Author.Id.ToString(), Name = post.Author.Name, AvatarUrl = post.Author.AvatarUrl },
                Description = post.Description,
                MultimediaUrls = { post.MultimediaUrls },
                Tags = { post.Tags },
                CreatedAt = post.CreatedAt.AsUtc().ToTimestamp(),
                LikesCount = post.LikesCount,
                LikedByUser = post.LikedByUser,
                CommentsCount = post.CommentsCount
            }) },
            Pagination = new PaginationDto { PageSize = postsPaginated.PageSize, Cursor = postsPaginated.NextCursor }.ToProtoPaginationReply()
        };

        return reply;
    }
}
