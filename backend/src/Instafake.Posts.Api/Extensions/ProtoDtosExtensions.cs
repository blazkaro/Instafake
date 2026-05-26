using Google.Protobuf.WellKnownTypes;

namespace Instafake.Posts.Api.Extensions;

public static class ProtoDtosExtensions
{
    extension(Protos.CursorPagination? protoCursor)
    {
        public Application.Queries.Pagination.CursorPagination? ToCursorPagination()
        {
            return protoCursor is not null ? new()
            {
                Id = protoCursor.Id,
                LastItemCreatedAt = protoCursor.LastItemCreatedAt.ToDateTime(),
                PageSize = protoCursor.PageSize
            } : null;
        }
    }

    extension(Application.Queries.Pagination.CursorPagination? cursor)
    {
        public Protos.CursorPagination? ToProtoCursor()
        {
            return cursor is not null ? new()
            {
                Id = cursor.Id,
                LastItemCreatedAt = cursor.LastItemCreatedAt.AsUtc().ToTimestamp(),
                PageSize = cursor.PageSize
            } : null;
        }
    }
}
