export interface Pagination{
    pageSize: number;
    cursor: CursorPagination | null;
}

export interface CursorPagination {
    lastItemCreatedAt: Date;
    id: string;
}