namespace Instafake.Posts.Infrastructure.Extensions;

internal static class EntitiesMapping
{
    extension(Domain.Entities.Post post)
    {
        public Infrastructure.Entities.Post ToEntity()
        {
            return new()
            {
                Id = post.Id,
                AuthorId = post.AuthorId,
                MultimediaUrls = [.. post.MultimediaUrls],
                Description = post.Description,
                Tags = [.. post.Tags.Select(tag => new Infrastructure.Entities.PostTag
                {
                    PostId = post.Id,
                    Tag = tag
                })],
                CreatedAt = post.CreatedAt
            };
        }
    }

    extension(Domain.Entities.Comment comment)
    {
        public Infrastructure.Entities.PostComment ToEntity()
        {
            return new()
            {
                Id = comment.Id,
                PostId = comment.PostId,
                AuthorId = comment.AuthorId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
            };
        }
    }

    extension(Domain.Entities.Author author)
    {
        public Infrastructure.Entities.User ToEntity()
        {
            return new()
            {
                Id = author.Id,
                Name = author.Name,
                AvatarUrl = author.AvatarUrl
            };
        }
    }

    extension(Domain.Entities.PostLike postLike)
    {
        public Infrastructure.Entities.PostLike ToEntity()
        {
            return new()
            {
                PostId = postLike.PostId,
                UserId = postLike.UserId
            };
        }
    }
}
