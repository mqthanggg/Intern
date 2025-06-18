import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { environment } from '../../../../environments/environment';
import { catchError, delay, finalize, mergeMap, of, throwError } from 'rxjs';

@Component({
  selector: 'app-station-delete',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './station-delete.component.html',
  styleUrl: './station-delete.component.css'
})
export class StationDeleteComponent implements OnInit{
  stationName: string = "";
  isDeleting = false;
  errorMessage: string = "";
  isOpenErrorModal = false
  @Input('id') stationId: number = -1;

  constructor(private router:Router, private http: HttpClient){}
  ngOnInit(): void {
      const rootSnapshot = this.router.routerState.snapshot.root
      this.stationName = rootSnapshot.queryParams['name']
  }
  deleteStation(){
    this.isDeleting = true
    this.http.delete(`${environment.serverURI}/station/${this.stationId}`,{
      observe: 'response'
    }).pipe(
      mergeMap((res) => of(res).pipe(delay(1000))),
      catchError((err) => of(err).pipe(
        mergeMap(() => throwError(() => err))
      )),
      finalize(() => {
        this.isDeleting = false
      })
    ).subscribe({
      next: (res) => {
        if (res.status === 200){
          this.router.navigate(['user/stations']).then((_) => {
            window.location.reload()
          })
        }
      },
      error: (err: HttpErrorResponse) => {
        this.isOpenErrorModal = true
      }
    })
  }
}
