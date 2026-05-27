import { HttpParams } from '@angular/common/http';
import { CursorPagination } from '../pagination/cursor-pagination';

export function addCursorPaginationParams(
    params: HttpParams,
    cursor?: CursorPagination | null
): HttpParams {
    if (cursor == null || cursor == undefined) return params;

    if (cursor.id != null) {
        params = params.set('cursor.id', cursor.id);
    }

    if (cursor.lastItemCreatedAt != null) {
        params = params.set('cursor.lastItemCreatedAt', cursor.lastItemCreatedAt.toISOString());
    }

    if (cursor.pageSize != null) {
        params = params.set('cursor.pageSize', cursor.pageSize);
    }

    return params;
}