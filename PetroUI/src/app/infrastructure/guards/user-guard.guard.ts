import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router, UrlTree } from '@angular/router';
import { environment } from '../../../environments/environment';
import { catchError, map, of } from 'rxjs';

export const userGuard: CanActivateChildFn | CanActivateFn = (childRoute, state) => {
  const http = inject(HttpClient)
  const router = inject(Router)
  if (localStorage.getItem('refresh') == null || localStorage.getItem('jwt') == null)
    return router.parseUrl('login')
  return true
};
