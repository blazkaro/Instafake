import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiConfig, ApiPaths } from '../../api-config';
import { PaginatedResponse, PaginationRequest } from '../../pagination/cursor-pagination';
import { Post } from '../models/post';
import { addPaginationParams, serializePaginatedResponse } from '../../pagination/pagination.utils';

export interface CreatePostResponse {
  id: string;
}

@Injectable({
  providedIn: 'root',
})
export class PostsService {
  private http = inject(HttpClient);

  getPosts(tags: string[] = [], authorName: string | null = null, pagination: PaginationRequest): Observable<PaginatedResponse<Post>> {
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
    return this.http.get<PaginatedResponse<Post>>(`${apiConfig.baseUrl}${ApiPaths.Posts}`, {
      withCredentials: true,
      params: params
    }).pipe(
      serializePaginatedResponse()
    );
  }

  /**
  * @returns Observable with post id
  */
  createPost(multimediaUrls: URL[], description: string, tags: string[]): Observable<CreatePostResponse> {
    return this.http.post<CreatePostResponse>(`${apiConfig.baseUrl}${ApiPaths.Posts}`, {
      multimediaUrls: multimediaUrls.map(val => val.href),
      description: description,
      tags: tags
    }, { withCredentials: true });
  }
}
