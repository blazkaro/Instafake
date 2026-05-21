import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { apiConfig, ApiPaths } from '../../shared/api-config';
import { AuthStatus } from './results/auth-result';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient)

  checkAuthStatus(): Observable<AuthStatus> {
    return this.http
      .get<AuthStatus>(`api${ApiPaths.Auth}/status`, { withCredentials: true })
  }
}
