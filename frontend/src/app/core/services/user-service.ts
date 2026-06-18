import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { catchError, distinctUntilKeyChanged, Observable, of, throwError } from 'rxjs';
import { apiConfig, ApiPaths } from '../../shared/api-config';
import { User } from '../models/user';
import { AuthStatus } from './results/auth-result';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);

  private userResource = rxResource({
    stream: () => this.http
      .get<User>(`${apiConfig.baseUrl}${ApiPaths.Auth}/user`, { withCredentials: true })
      .pipe(
        distinctUntilKeyChanged('id'),
        catchError((err: HttpErrorResponse) => {
          if (err.status === 401) return of(null);
          return throwError(() => err);
        })
      )
  });

  checkAuthStatus(): Observable<AuthStatus> {
    return this.http
      .get<AuthStatus>(`${apiConfig.baseUrl}${ApiPaths.Auth}/status`, { withCredentials: true })
  }

  user = this.userResource.value.asReadonly();
}
