import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { ApiPaths } from '../../shared/api-config';
import { catchError, distinctUntilKeyChanged } from 'rxjs';
import { Post } from '../../shared/post/models/post';
import { CursorPagination } from '../../shared/pagination/cursor-pagination';
import { addCursorPaginationParams } from '../../shared/utils/http-params.utils';

export interface GetPostsResponse {
  posts: Post[];
  nextCursor: CursorPagination | null;
}

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  private http = inject(HttpClient);

  getPosts(tags: string[] = [], authorId: string | null = null, cursor: CursorPagination | null = null) {
    let params = new HttpParams();
    if (tags != undefined && tags?.length > 0) {
      for (const tag of tags) {
        params = params.append('tags', tag);
      }
    }

    if (authorId != undefined) {
      params.set('authorId', authorId);
    }

    addCursorPaginationParams(params, cursor);

    return this.http.get<GetPostsResponse>(`api${ApiPaths.Posts}`, {
      withCredentials: true,
      params: params
    });
  }
}
