import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';

export const administratorGuard: CanActivateFn | CanActivateChildFn = (route, state) => {
  const router = inject(Router)
  const [refreshToken, jwt, role] = [localStorage.getItem('refresh'),localStorage.getItem('jwt'),localStorage.getItem('role')]
  if (refreshToken == null || jwt == null || role != 'administrator')
    return router.parseUrl('login')
  return true
};
