import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { apiConfig, ApiPaths } from '../../api-config';
import { PostComment } from '../models/post-comment';
import { PaginatedResponse, PaginationRequest, PaginationResponse } from '../../pagination/cursor-pagination';
import { Observable } from 'rxjs';
import { addPaginationParams, serializePaginatedResponse } from '../../pagination/pagination.utils';

export interface CreateCommentResponse {
  id: string;
}

@Injectable({
  providedIn: 'root',
})
export class PostCommentsService {
  private readonly http = inject(HttpClient);

  getComments(postId: string, pagination: PaginationRequest): Observable<PaginatedResponse<PostComment>> {
    let params = addPaginationParams(new HttpParams(), pagination);
    return this.http.get<PaginatedResponse<PostComment>>(`${apiConfig.baseUrl}${ApiPaths.Posts}/${postId}/comments`, {
      withCredentials: true,
      params: params
    }).pipe(
      serializePaginatedResponse()
    );
  }

  createComment(postId: string, content: string): Observable<CreateCommentResponse> {
    return this.http.post<CreateCommentResponse>(`${apiConfig.baseUrl}${ApiPaths.Posts}/${postId}/comments`, { content }, {
      withCredentials: true
    });
  }
}
