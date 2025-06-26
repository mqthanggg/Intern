import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { TitleService } from '../../../infrastructure/services/title.service';
import { environment } from './../../../../environments/environment';
import { delay, mergeMap,catchError,finalize,of,throwError } from 'rxjs';
import { ElementDraggableDirective, ElementDraggableSectionDirective } from './../../../shared/directives/element-draggable.directive';
import { ActivatedRoute } from '@angular/router';
import { DispenserRecord } from './dispenser-record';
import { TankRecord } from './tank-record';
import { NgClass } from '@angular/common';
import { LogRecord } from './log-record';

@Component({
  selector: 'app-station',
  standalone: true,
  imports: [ElementDraggableDirective, ElementDraggableSectionDirective, NgClass],
  templateUrl: './station.component.html',
  styleUrl: './station.component.css'
})
export class StationComponent implements OnInit{
  @Input() id: number = -1;
  stationName: string = "";
  stationAddress: string = "";
  isDispenserLoading = false;
  isTankLoading = false;
  isLogLoading = false;
  dispenserList: DispenserRecord[] = [];
  tankList: TankRecord[] = [];
  logList: LogRecord[] = [];
  _temp_statusList: number[] = [];
  constructor(private http: HttpClient, private titleServer: TitleService, private route:ActivatedRoute){}
  ngOnInit(): void {
    const snapshot = this.route.snapshot
    this.stationName = snapshot.queryParams['name']
    this.stationAddress = snapshot.queryParams['address']
    setTimeout(() => {
      this.titleServer.updateTitle(this.stationName)
      this.isDispenserLoading = true
      this.http.get(environment.serverURI+`/dispenser/station/${this.id}`,{observe: "response"}).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),//Simulating delay
        catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),//Simulating delay
        finalize(() => {
          this.isDispenserLoading = false
        })).subscribe({
          next: (res: HttpResponse<any>) => {
            this.dispenserList = res.body
          },
          error: (err: HttpErrorResponse) => {

          }
      })
      this.isTankLoading = true
      this.http.get(environment.serverURI+`/tank/station/${this.id}`,{observe: "response"}).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),//Simulating delay
        catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),//Simulating delay
        ).subscribe({
          next: (res: HttpResponse<any>) => {
            this.tankList = res.body
          },
          error: (err: HttpErrorResponse) => {

          }
      })
      this.http.get(environment.serverURI+`/log/station/${this.id}`,{observe: "response"}).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),//Simulating delay
        catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),//Simulating delay
        finalize(() => {this.isLogLoading = false})
        ).subscribe({
          next: (res: HttpResponse<any>) => {
            this.logList = res.body
          },
          error: (err: HttpErrorResponse) => {

          }
      })
      setInterval(() => {
        this.simulation()
        this.isTankLoading = false
      },3000)
    },0)
  }
  simulatingStatus: "IDLE" | "PUMP" | "RESET"  = "IDLE"
  simulatingLiter = 0
  simulatingTotalPrice = 0
  simulation(){
    this.simulatingStatus = "IDLE"
    this._temp_statusList = new Array(this.tankList.length).fill(0).map(() => Math.round(Math.random() * 100) % 101)    
    const update = () => {
      if (this.simulatingTotalPrice < 100000){
        this.simulatingStatus = "PUMP"
        this.simulatingLiter += 0.1
        this.simulatingLiter = Math.round(this.simulatingLiter*10)/10
        this.simulatingTotalPrice += 50
        requestAnimationFrame(update)
      }
      else {
        this.simulatingStatus = "RESET"
        setTimeout(() => {
          this.simulatingStatus = "IDLE"
          this.simulatingLiter = 0
          this.simulatingTotalPrice = 0
        },2000)
      }
    }
    update()
  }
}
