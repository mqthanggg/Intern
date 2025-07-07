declare let BigNumber: any;
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { TitleService } from '../../../infrastructure/services/title.service';
import { environment } from './../../../../environments/environment';
import { delay, mergeMap,catchError,finalize,of,throwError, Subscription } from 'rxjs';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket'
import { ElementDraggableDirective, ElementDraggableSectionDirective } from './../../../shared/directives/element-draggable.directive';
import { ActivatedRoute } from '@angular/router';
import { DispenserRecord, WSDispenserRecord } from './dispenser-record';
import { TankRecord, WSTankRecord } from './tank-record';
import { NgClass } from '@angular/common';
import { LogRecord } from './log-record';

@Component({
  selector: 'app-station',
  standalone: true,
  imports: [ElementDraggableDirective, ElementDraggableSectionDirective, NgClass],
  templateUrl: './station.component.html',
  styleUrl: './station.component.css'
})
export class StationComponent implements OnInit, OnDestroy{
  @Input() id: number = -1;
  stationName: string = "";
  stationAddress: string = "";
  isDispenserLoading = false;
  isTankLoading = false;
  isLogLoading = false;
  dispenserList: DispenserRecord[] = [];
  dispenserSocket: {[key: string]: WebSocketSubject<WSDispenserRecord>} = {}
  tankSocket: {[key: string]: WebSocketSubject<WSTankRecord>} = {}
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
        mergeMap((res) => of(res).pipe(delay(1000))), //Simulating delay
        catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))), //Simulating delay
        finalize(() => {
          this.isDispenserLoading = false
        })).subscribe({
          next: (res: HttpResponse<any>) => {
            this.dispenserList = res.body
            this.dispenserList.forEach((value, index) => {
              this.dispenserSocket[value.dispenserId] = webSocket<WSDispenserRecord>(environment.wsServerURI + `/ws/dispenser/${value.dispenserId}`)
              this.dispenserSocket[value.dispenserId].subscribe({
                next: (res: WSDispenserRecord) => {
                  this.dispenserList[index].liter = res.liter
                  this.dispenserList[index].totalAmount = res.price
                  this.dispenserList[index].status = res.state
                },
                error: (err) => {
                  console.error(`Error at dispenser ${value.dispenserId}: ${err}`);
                }
              })
            })
          },
          error: (err: HttpErrorResponse) => {

          }
      })
      this.isTankLoading = true
      this.http.get(environment.serverURI+`/tank/station/${this.id}`,{observe: "response"}).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),//Simulating delay
        catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),//Simulating delay
        finalize(() => {this.isTankLoading = false})
        ).subscribe({
          next: (res: HttpResponse<any>) => {
            this.tankList = res.body
            this.tankList.forEach((value, index) => {
              this.tankSocket[value.tankId] = webSocket<WSTankRecord>(environment.wsServerURI + `/ws/tank/${value.tankId}`)
              this.tankSocket[value.tankId].subscribe({
                next: (res: WSTankRecord) => {
                  this.tankList[index].currentVolume = res.current_volume
                  const vMax = new BigNumber(this.tankList[index].maxVolume)
                  this.tankList[index].percentage = new BigNumber(res.current_volume).dividedBy(vMax).times(100).toFixed(2).toString()
                },
                error: (err) => {
                  console.error(`Error at tank ${value.tankId}: ${err}`);
                }
              })
            })
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
    },0)
    window.onbeforeunload = () => this.ngOnDestroy()
  }
  ngOnDestroy(): void {
    console.log("Calling ngOnDestroy");
    
      for(const key in this.dispenserSocket){
        this.dispenserSocket[key].complete()
      }
      for(const key in this.tankSocket){
        this.tankSocket[key].complete()
      }
  }
}
