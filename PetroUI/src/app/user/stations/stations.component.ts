import { Component, OnInit } from '@angular/core';
import { TitleService } from '../title.service';
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { StationRecord } from './station-record';
import { environment } from '../../../environments/environment';
import { delay, mergeMap,catchError,finalize,of,throwError} from 'rxjs';
import { RouterLink, RouterOutlet } from '@angular/router';
import { StationDeleteComponent } from './station-delete/station-delete.component';

@Component({
  selector: 'app-stations',
  standalone: true,
  imports: [RouterLink, StationDeleteComponent, RouterOutlet],
  templateUrl: './stations.component.html',
  styleUrl: './stations.component.css'
})
export class StationsComponent implements OnInit{
  constructor(private titleService: TitleService,private http:HttpClient){}
  stationList: StationRecord[] = []
  isStationLoading = false
  ngOnInit(): void {
    setTimeout(() => {
      this.isStationLoading = true
      this.titleService.updateTitle("Stations")
      this.http.get(environment.serverURI + '/stations',{observe: "response"}).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),//Simulating delay
        catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),//Simulating delay
        finalize(() => {
          this.isStationLoading = false
        })).subscribe({
          next: (res: HttpResponse<Object>) => {
            this.stationList = res.body as StationRecord[]
          },
          error: (err: HttpErrorResponse) => {
            
          }
        })
    },0)
  }
}
