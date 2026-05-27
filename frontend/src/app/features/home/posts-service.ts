import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { ApiPaths } from '../../shared/api-config';
import { catchError, distinctUntilKeyChanged } from 'rxjs';
import { Post } from '../../shared/post/models/post';
import { CursorPagination, Pagination } from '../../shared/pagination/cursor-pagination';
import { addPaginationParams } from '../../shared/utils/http-params.utils';

export interface GetPostsResponse {
  posts: Post[];
  pagination: Pagination;
}

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  private http = inject(HttpClient);

  getPosts(tags: string[] = [], authorId: string | null = null, pagination: Pagination) {
    let params = new HttpParams();
    if (tags != undefined && tags?.length > 0) {
      for (const tag of tags) {
        params = params.append('tags', tag);
      }
    }

    if (authorId != undefined) {
      params.set('authorId', authorId);
    }

    params = addPaginationParams(params, pagination);
    return this.http.get<GetPostsResponse>(`api${ApiPaths.Posts}`, {
      withCredentials: true,
      params: params
    });
  }
}
