import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';
import { environment } from '../environments/environment';
import { catchError, map, of } from 'rxjs';

export const userGuard: CanActivateChildFn | CanActivateFn = (childRoute, state) => {
  const http = inject(HttpClient)
  const router = inject(Router)
  console.log('Token refreshed');
  
  return http.post(`${environment.serverURI}/refresh`,{
    refreshToken: localStorage.getItem('refresh')
  },{
    observe: "response"
  }).pipe(
    map((value: HttpResponse<any>) => {
      if (value.status !== 200){
        router.navigate(['/login'])
        return false
      }
      localStorage.setItem('jwt', value.body.token)
      return true
    }),
    catchError((error: any) => {
      console.log(error);
      router.navigate(['/login'])
      return of(false)
    })
  )
};
