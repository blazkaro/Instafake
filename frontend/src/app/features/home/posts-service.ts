import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map } from 'rxjs';
import { ApiPaths } from '../../shared/api-config';
import { PaginationRequest, PaginationResponse } from '../../shared/pagination/cursor-pagination';
import { Post } from '../../shared/post/models/post';
import { addPaginationParams } from '../../shared/utils/http-params.utils';

export interface GetPostsResponse {
  posts: Post[];
  pagination: PaginationResponse;
}

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  private http = inject(HttpClient);

  getPosts(tags: string[] = [], authorName: string | null = null, pagination: PaginationRequest) {
    let params = new HttpParams();
    if (tags != undefined && tags?.length > 0) {
      for (const tag of tags) {
        params = params.append('tags', tag);
      }
    }

    if (authorName != undefined) {
      params = params.set('authorName', authorName);
    }

    params = addPaginationParams(params, pagination);
    return this.http.get<GetPostsResponse>(`api${ApiPaths.Posts}`, {
      withCredentials: true,
      params: params
    }).pipe(
      map((response) => {
        const cursor = response.pagination.nextCursor;
        if (cursor?.lastItemCreatedAt) {
          cursor.lastItemCreatedAt = new Date(cursor.lastItemCreatedAt);
        }

        return response;
      })
    );
  }
}
