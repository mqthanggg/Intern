import { HttpClient, HttpResponse } from "@angular/common/http";
import { inject } from "@angular/core";
import { RedirectFunction } from "@angular/router";
import { environment } from "../../../environments/environment";

export const autoRedirection : RedirectFunction = () => {
  if (localStorage.getItem('jwt')){
    const http = inject(HttpClient)
    const subscription = http.post(
      environment.serverURI + '/refresh',
      {
        token: localStorage.getItem('jwt'),
        refreshToken: localStorage.getItem('refresh')
      },
      {
        observe: 'response'
      }
    ).subscribe({
      next: (res: HttpResponse<any>) => {
        localStorage.setItem('jwt',res.body?.token ?? '')
        return `/${localStorage.getItem('role')}`
      },
      error: (_) => {
        return '/login'
      }
    })
  }
  return '/login';
}
