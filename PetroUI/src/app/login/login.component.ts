import { HttpClient, HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import {FormControl, ReactiveFormsModule, UntypedFormGroup, Validators} from '@angular/forms'
import { environment } from '../../environments/environment';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, NgClass],
  templateUrl: './login.component.html',
  styles: ``
})
export class LoginComponent {
  constructor(private http:HttpClient){}
  loginForm = new UntypedFormGroup({
    username: new FormControl('',[Validators.required]),
    password: new FormControl('',[Validators.required])
  })
  loginFormSubmit(){
    this.http.post(
      `${environment.serverURI}/login`,
      this.loginForm.value,
      {observe: 'response'}
    ).pipe().subscribe((res: HttpResponse<any>) => {
      if (res.status === 200)
        console.log("Login success!");
        
    })
  }
}
