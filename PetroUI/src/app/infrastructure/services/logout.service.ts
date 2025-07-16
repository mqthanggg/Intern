import { inject, Injectable } from "@angular/core"
import { Router } from "@angular/router"

@Injectable({
  providedIn: 'root'
})
export class LogoutService{
  constructor (
    private router:Router
  ){

  }
  logout = async () => {
    localStorage.clear()
    const res = await this.router.navigate(['/login'])
    return res
  }
}
