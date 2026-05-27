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
                LastItemCreatedAt = protoCursor.LastItemCreatedAt.ToDateTime()
            } : null;
        }
    }


    extension(CursorPaginationDto? cursor)
    {
        public ServicesProtos.Shared.CursorPagination? ToProtoCursorPagination()
        {
            return cursor is not null ? new()
            {
                Id = cursor.Id,
                LastItemCreatedAt = cursor.LastItemCreatedAt.AsUtc().ToTimestamp()
            } : null;
        }
    }

    extension(ServicesProtos.Shared.PaginationRequest? paginationRequest)
    {
        public PaginationDto? ToPaginationDto()
        {
            return paginationRequest is not null ? new()
            {
                PageSize = paginationRequest.PageSize,
                Cursor = paginationRequest.Cursor.ToCursorPagination()
            } : null;
        }
    }

    extension(ServicesProtos.Shared.PaginationReply? paginationReply)
    {
        public PaginationResultDto? ToPaginationResultDto()
        {
            return paginationReply is not null ? new()
            {
                PageSize = paginationReply.PageSize,
                NextCursor = paginationReply.NextCursor.ToCursorPagination()
            } : null;
        }
    }

    extension(PaginationDto? paginationDto)
    {
        public ServicesProtos.Shared.PaginationRequest? ToProtoPaginationRequest()
        {
            return paginationDto is not null ? new()
            {
                PageSize = paginationDto.PageSize,
                Cursor = paginationDto.Cursor.ToProtoCursorPagination()
            } : null;
        }
    }
}
