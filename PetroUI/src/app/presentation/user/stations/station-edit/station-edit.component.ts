import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { environment } from './../../../../../environments/environment';
import { catchError, delay, finalize, mergeMap, of, throwError } from 'rxjs';

@Component({
  selector: 'app-station-edit',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './station-edit.component.html',
  styleUrl: './station-edit.component.css'
})
export class StationEditComponent implements OnInit {
  isOpenErrorModal = false
  @Input('id') stationId: number = -1;
  stationAddress: string = "";
  stationName: string = "";
  isUpdateLoading = false
  stationForm = new UntypedFormGroup({
    name: new FormControl({value: '', disabled: true}, [Validators.required]),
    address: new FormControl({value: '', disabled: true}, [Validators.required])
  })
  constructor(private router: Router, private http: HttpClient){}
  ngOnInit(): void {
    const routeSnapshot = this.router.routerState.snapshot.root
    this.stationAddress = routeSnapshot.queryParams['address']
    this.stationName = routeSnapshot.queryParams['name']
    this.stationForm.setValue({name: this.stationName, address: this.stationAddress})
  }
  navigateBack(){
    this.router.navigate(['/user/stations'])
  }
  formSubmit(){
    this.isUpdateLoading = true
    this.http.put(`${environment.serverURI}/station/${this.stationId}`,this.stationForm.getRawValue(),{
      observe: 'response'
    }).pipe(
      mergeMap((val) => of(val).pipe(delay(1000))),
      catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),
      finalize(() => {this.isUpdateLoading = false})
    ).subscribe({
      next: (val) => {
        if (val.status === 200){
          this.router.navigate(['/user/stations']).then((_) => {
            window.location.reload()
          })
        }
      },
      error: (err: HttpErrorResponse) => {
        console.error(err);
      }
    })
  }
}
