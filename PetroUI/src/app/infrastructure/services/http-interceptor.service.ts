import { HttpClient, HttpErrorResponse, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, map, of, switchMap, tap, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

const ignoredURL: string[] = ['login']

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router)
  const http = inject(HttpClient)  
  if (ignoredURL.find((value) => {
    return value == req.url.split('/').at(-1)
  }) !== undefined)
    return next(req)
  else{
    req = req.clone({
      headers: req.headers.append('Authorization', `Bearer ${localStorage.getItem('jwt')}`)
    })
    return next(req).pipe(
      catchError((err: HttpErrorResponse) => {
        if (
          console.error(err.headers),
          err.headers?.get('www-authenticate')?.split(',')[1].match(/The token expired at '([^']+)'/)?.[1] &&
          req.url.split('/').at(-1) !== 'refresh'
        ) {        
          return http.post(`${environment.serverURI}/refresh`,{
            token: localStorage.getItem('jwt'),
            refreshToken: localStorage.getItem('refresh')
          },{
            observe: "response"
          }
          ).pipe(
            tap((value: HttpResponse<any>) => {
              localStorage.setItem('jwt', value.body.token)
            }),
            switchMap(() => {
              const newReq = req.clone({
                headers: req.headers.append('Authorization', `Bearer ${localStorage.getItem('jwt')}`)
              })
              return next(newReq)
            }),
            catchError((err) => {
              router.navigate(['/login'])
              return throwError(()=>err)
            })
          )
        }
        else
          return throwError(() => err)
      })
    )
  }
}
