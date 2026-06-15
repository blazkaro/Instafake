using Instafake.Posts.Application.Services;
using MediatR;

namespace Instafake.Posts.Application.Commands.Handlers;

internal class UpdateLikeCommandHandler(ILikeService likeService) : IRequestHandler<UpdateLikeCommand, bool>
{
    private readonly ILikeService _likeService = likeService;

    public async Task<bool> Handle(UpdateLikeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Like)
            {
                await _likeService.Like(request.PostId, request.UserId, cancellationToken);
            }
            else
            {
                await _likeService.Dislike(request.PostId, request.UserId, cancellationToken);
            }
        }
        catch
        {
            return false;
        }

        return true;
    }
}
