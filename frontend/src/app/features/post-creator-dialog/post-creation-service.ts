import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiConfig, ApiPaths } from '../../shared/api-config';

@Injectable({
  providedIn: 'root',
})
export class PostCreationService {
  private readonly http = inject(HttpClient);

  /**
   * @returns Observable with post id
   */
  createPost(multimedia: File[], description: string, tags: string[]): Observable<string> {
    //return this.http.post(`api${ApiPaths.Posts}`, {})
    throw new Error("Not implemented");
  }
}
