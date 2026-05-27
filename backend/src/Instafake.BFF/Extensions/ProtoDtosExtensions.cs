using Google.Protobuf.WellKnownTypes;
using Instafake.BFF.Controllers.SharedDtos;

namespace Instafake.BFF.Extensions;

public static class ProtoDtosExtensions
{
    extension(ServicesProtos.Shared.CursorPagination? protoCursor)
    {
        public CursorPaginationDto? ToCursorPagination()
        {
            return protoCursor is not null ? new()
            {
                Id = protoCursor.Id,
                LastItemCreatedAt = protoCursor.LastItemCreatedAt.ToDateTime(),
                PageSize = protoCursor.PageSize
            } : null;
        }
    }

    extension(CursorPaginationDto? cursor)
    {
        public ServicesProtos.Shared.CursorPagination? ToProtoCursor()
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
