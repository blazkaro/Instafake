import { HttpParams } from '@angular/common/http';
import { PaginatedResponse, PaginationRequest, PaginationResponse } from '../pagination/cursor-pagination';
import { map } from 'rxjs';

export function addPaginationParams(
    params: HttpParams,
    pagination?: PaginationRequest | null
): HttpParams {
    if (pagination == null || pagination == undefined) return params;

    if (pagination.pageSize != null) {
        params = params.set('pagination.pageSize', pagination.pageSize);
    }

    if (pagination.cursor == null)
        return params;

    if (!pagination.cursor.lastItemCreatedAt || !pagination.cursor.id) {
        throw new Error("When cursor pagination is used, date of last item's creation and its id must be present");
    }

    params = params.set('pagination.cursor.lastItemCreatedAt', pagination.cursor.lastItemCreatedAt.toISOString());
    params = params.set('pagination.cursor.id', pagination.cursor.id);
    return params;
}

export function serializePaginatedResponse<TData>() {
    return map((response: PaginatedResponse<TData>) => {
        const cursor = response.pagination.nextCursor;
        if (cursor?.lastItemCreatedAt) {
            cursor.lastItemCreatedAt = new Date(cursor.lastItemCreatedAt);
        }

        return response;
    });
}