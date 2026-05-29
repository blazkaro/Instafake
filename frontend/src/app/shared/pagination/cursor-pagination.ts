export interface PaginationRequest{
    pageSize: number;
    cursor: CursorPagination | null;
}

export interface PaginationResponse {
    pageSize: number;
    nextCursor: CursorPagination | null;
}

export interface CursorPagination {
    lastItemCreatedAt: Date;
    id: string;
}