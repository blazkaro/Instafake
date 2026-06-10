import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ApiPaths } from '../../api-config';
import { PaginationRequest, PaginationResponse } from '../../pagination/cursor-pagination';
import { Post } from '../models/post';
import { addPaginationParams } from '../../utils/http-params.utils';

export interface GetPostsResponse {
  posts: Post[];
  pagination: PaginationResponse;
}

export interface CreatePostResponse {
  id: string;
}

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  private http = inject(HttpClient);

  getPosts(tags: string[] = [], authorName: string | null = null, pagination: PaginationRequest): Observable<GetPostsResponse> {
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

  /**
  * @returns Observable with post id
  */
  createPost(multimediaUrls: URL[], description: string, tags: string[]): Observable<CreatePostResponse> {
    return this.http.post<CreatePostResponse>(`api${ApiPaths.Posts}`, {
      multimediaUrls: multimediaUrls.map(val => val.href),
      description: description,
      tags: tags
    }, { withCredentials: true });
  }
}
