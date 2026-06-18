import { HttpClient, HttpResponse, HttpStatusCode } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { apiConfig, ApiPaths } from '../../api-config';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class LikesService {
  private readonly http = inject(HttpClient);

  like(postId: string): Observable<boolean> {
    return this.updateLikeState(postId, true);
  }

  dislike(postId: string): Observable<boolean> {
    return this.updateLikeState(postId, false);
  }

  private updateLikeState(postId: string, like: boolean): Observable<boolean> {
    return this.http.put(`${apiConfig.baseUrl}${ApiPaths.Likes}`, { postId, like }, { withCredentials: true, observe: 'response' }).pipe(
      map((response: HttpResponse<{}>) => {
        if (response.status !== HttpStatusCode.Ok) {
          return false;
        }

        return true;
      }),
      catchError((err) => of(false))
    );
  }
}
