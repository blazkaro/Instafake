import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable, resource, ResourceRef } from '@angular/core';
import { ApiPaths } from '../../shared/api-config';
import { AuthStatus } from './results/auth-result';
import { BehaviorSubject, catchError, distinctUntilKeyChanged, Observable, of, tap, throwError } from 'rxjs';
import { User } from '../models/user';
import { rxResource } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);

  private userResource = rxResource({
    stream: () => this.http
      .get<User>(`api${ApiPaths.Auth}/user`, { withCredentials: true })
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
      .get<AuthStatus>(`api${ApiPaths.Auth}/status`, { withCredentials: true })
  }

  user = this.userResource.value.asReadonly();
}
