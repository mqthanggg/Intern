import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router, UrlTree } from '@angular/router';
import { environment } from '../../../environments/environment';
import { catchError, map, of } from 'rxjs';

export const userGuard: CanActivateChildFn | CanActivateFn = (childRoute, state) => {
  const http = inject(HttpClient)
  const router = inject(Router)
  return localStorage.getItem('refresh') !== null && localStorage.getItem('jwt') !== null && http.post(`${environment.serverURI}/refresh`,{
    refreshToken: localStorage.getItem('refresh')
  },{
    observe: "response"
  }).pipe(
    map((value: HttpResponse<any>) => {
      console.log('Token refreshed');
      localStorage.setItem('jwt', value.body.token)
      return true
    }),
    catchError((_: any) => {
      return of(router.parseUrl(`/login`))
    })
  )
};
