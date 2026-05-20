using Grpc.Core;
using Instafake.Posts.Api.Protos;

namespace Instafake.Posts.Api.Services;

public class PosterService(ILogger<PosterService> logger) : Poster.PosterBase
{
    public override Task<CreatePostReply> CreatePost(CreatePostRequest request, ServerCallContext context)
    {
        return base.CreatePost(request, context);
    }

    public override Task<GetPostCommentsReply> GetPostComments(GetPostCommentsRequest request, ServerCallContext context)
    {
        return base.GetPostComments(request, context);
    }

    public override Task<GetPostsReply> GetPosts(GetPostsRequest request, ServerCallContext context)
    {
        return base.GetPosts(request, context);
    }
}
