using Instafake.BFF.Controllers.Post.Dtos;
using Instafake.BFF.Extensions;
using Instafake.BFF.ServicesProtos.Post;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Instafake.BFF.Controllers.Post;

[Route("[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class PostsController(Poster.PosterClient client) : ControllerBase
{
    private readonly Poster.PosterClient _client = client;

    [HttpPost]
    public async Task<IActionResult> CreatePostAsync([FromBody] Dtos.CreatePostRequest dto, CancellationToken cancellationToken)
    {
        var reply = await _client.CreatePostAsync(new ServicesProtos.Post.CreatePostRequest
        {
            Description = dto.Description,
            Tags = { dto.Tags },
            MultimediaUrls = { dto.MultimediaUrls }
        }, cancellationToken: cancellationToken);

        return Ok(new CreatePostResponse(reply.Id));
    }

    [HttpGet]
    public async Task<IActionResult> GetPostsAsync([FromQuery] Dtos.GetPostsRequest dto, CancellationToken cancellationToken)
    {
        var protoRequest = new ServicesProtos.Post.GetPostsRequest
        {
            Pagination = dto.Pagination.ToProtoPaginationRequest()
        };

        if (!string.IsNullOrEmpty(dto.AuthorName))
            protoRequest.AuthorName = dto.AuthorName;

        if (dto.Tags?.Length > 0)
            protoRequest.Tags.AddRange(dto.Tags);

        var reply = await _client.GetPostsAsync(protoRequest, cancellationToken: cancellationToken);
        return Ok(new PaginatedResponse<Dtos.PostDto>(
            [.. reply.Items.Select(protoPost => new Dtos.PostDto(
                protoPost.Id,
                new Dtos.AuthorDto(protoPost.Author.Id, protoPost.Author.Name, protoPost.Author.AvatarUrl),
                protoPost.Description,
                [.. protoPost.MultimediaUrls],
                [.. protoPost.Tags],
                protoPost.CreatedAt.ToDateTime(),
                protoPost.LikesCount,
                protoPost.LikedByUser,
                protoPost.CommentsCount))],
            reply.Pagination.ToPaginationResultDto()));
    }

    [HttpPost("{postId}/comments")]
    public async Task<IActionResult> CreateCommentAsync(string postId, [FromBody] Dtos.CreateCommenRequest dto, CancellationToken cancellationToken)
    {
        var reply = await _client.CreateCommentAsync(new ServicesProtos.Post.CreateCommentRequest
        {
            PostId = postId,
            Content = dto.Content
        }, cancellationToken: cancellationToken);

        return Ok(new CreateCommentResponse(reply.Id));
    }

    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetCommentsAsync(string postId, [FromQuery] Dtos.GetCommentsRequest dto, CancellationToken cancellationToken)
    {
        var reply = await _client.GetCommentsAsync(new ServicesProtos.Post.GetCommentsRequest
        {
            PostId = postId,
            Pagination = dto.Pagination.ToProtoPaginationRequest()
        }, cancellationToken: cancellationToken);

        return Ok(new PaginatedResponse<Dtos.CommentDto>(
            [..reply.Items.Select(protoComment => new Dtos.CommentDto(
                protoComment.Id,
                new Dtos.AuthorDto(protoComment.Author.Id, protoComment.Author.Name, protoComment.Author.AvatarUrl),
                protoComment.Content,
                protoComment.CreatedAt.ToDateTime()
                ))],
            reply.Pagination.ToPaginationResultDto()));
    }
}
