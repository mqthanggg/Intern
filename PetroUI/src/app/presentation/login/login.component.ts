import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import {FormControl, ReactiveFormsModule, UntypedFormGroup, Validators} from '@angular/forms'
import { environment } from '../../../environments/environment';
import { NgClass } from '@angular/common';
import { catchError, delay, finalize, mergeMap, of, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NgClass],
  styleUrl: './login.component.css',
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent {
showPassword: any;
  constructor(private http:HttpClient, private router: Router){}
  loginLoading = false
  alertOpen = false
  alertTimeout: undefined | any = undefined
  loginForm = new UntypedFormGroup({
    username: new FormControl('',[Validators.required]),
    password: new FormControl('',[Validators.required])
  })
  loginFormSubmit(){
    this.loginLoading = true
    this.http.post(
      `${environment.serverURI}/login`,
      this.loginForm.value,
      {
        observe: 'response',
        withCredentials: true
      }
    ).pipe(
      mergeMap((res) => of(res).pipe(delay(1000))),//Simulating delay
      catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),//Simulating delay
      finalize(() => {
        this.loginLoading = false
      })
    ).subscribe({
      next: (res: HttpResponse<any>) => {
        if (res.status === 200){
          localStorage.clear()
          localStorage.setItem('jwt',res.body.token);
          localStorage.setItem('refresh',res.body.refreshToken)
          localStorage.setItem('role',res.body.role)
          this.router.navigate([`/${res.body.role}`])
        }
      },
      error: (err: HttpErrorResponse) => {
        this.alertOpen = true
        this.alertTimeout = setTimeout(() => {
          this.alertOpen = false
        },3000)
      }
    })
  }
  closeModal(){
    if (this.alertOpen){
      this.alertOpen = false
      if (this.alertTimeout !== undefined) 
        clearTimeout(this.alertTimeout)
    }
  }
}