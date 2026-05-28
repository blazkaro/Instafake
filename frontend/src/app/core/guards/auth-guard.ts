import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { UserService } from '../services/user-service';

export const authGuard: CanActivateFn = (route, state) => {
  const userService = inject(UserService)
  const router = inject(Router)

  return userService.checkAuthStatus().pipe(
    map(authStatus => {
      return authStatus.isAuthenticated ? true : router.createUrlTree(['/signin'])
    })
  )
};
