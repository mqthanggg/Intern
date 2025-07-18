import { HttpClient, HttpResponse } from "@angular/common/http"
import { inject, Injectable } from "@angular/core"
import { Router } from "@angular/router"
import { environment } from "../../../environments/environment"

@Injectable({
  providedIn: 'root'
})
export class LogoutService{
  constructor (
    private router:Router,
    private http:HttpClient
  ){

  }
  logout = async () => {
    this.http.post(
      environment.serverURI + '/logout',
      {
        token: localStorage.getItem('jwt')
      },
      {
        withCredentials: true,
        observe: 'response'
      }
    ).
    subscribe({
      next: async (res: HttpResponse<any>) => {
        localStorage.clear()
        return await this.router.navigate(['/login'])
      },
      error: err => {
        console.error(err);
        return false;
      }
    })
    return false
  }
}
