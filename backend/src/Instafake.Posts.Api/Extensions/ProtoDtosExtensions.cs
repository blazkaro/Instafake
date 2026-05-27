using Google.Protobuf.WellKnownTypes;

namespace Instafake.Posts.Api.Extensions;

public static class ProtoDtosExtensions
{
    extension(Protos.Shared.CursorPagination? protoCursor)
    {
        public Application.Queries.Pagination.CursorPagination? ToCursorPagination()
        {
            return protoCursor is not null ? new()
            {
                Id = protoCursor.Id,
                LastItemCreatedAt = protoCursor.LastItemCreatedAt.ToDateTime()
            } : null;
        }
    }

    extension(Application.Queries.Pagination.CursorPagination? cursor)
    {
        public Protos.Shared.CursorPagination? ToProtoCursorPagination()
        {
            return cursor is not null ? new()
            {
                Id = cursor.Id,
                LastItemCreatedAt = cursor.LastItemCreatedAt.AsUtc().ToTimestamp()
            } : null;
        }
    }

    extension(Protos.Shared.PaginationRequest? paginationRequest)
    {
        public Application.Queries.Dtos.PaginationDto? ToPaginationDto()
        {
            return paginationRequest is not null ? new()
            {
                PageSize = paginationRequest.PageSize,
                Cursor = paginationRequest.Cursor.ToCursorPagination()
            } : null;
        }
    }

    extension(Application.Queries.Dtos.PaginationDto? paginationDto)
    {
        public Protos.Shared.PaginationReply? ToProtoPaginationReply()
        {
            return paginationDto is not null ? new()
            {
                PageSize = paginationDto.PageSize,
                NextCursor = paginationDto.Cursor.ToProtoCursorPagination()
            } : null;
        }
    }
}
