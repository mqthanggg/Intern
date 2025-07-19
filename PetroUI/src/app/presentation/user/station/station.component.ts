declare let BigNumber: any;
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { TitleService } from '../../../infrastructure/services/title.service';
import { environment } from './../../../../environments/environment';
import { delay, mergeMap, catchError, finalize, of, throwError, forkJoin } from 'rxjs';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket'
import { ActivatedRoute } from '@angular/router';
import { DispenserRecord, WSDispenserRecord } from './dispenser-record';
import { TankRecord, WSTankRecord } from './tank-record';
import { LogRecord, WSLogRecord } from './log-record';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration } from 'chart.js';
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { sumRevenueByLogType, sumRevenueByName } from './revenue-record';

@Component({
  selector: 'app-station',
  standalone: true,
  imports: [NgChartsModule, CommonModule, FormsModule],
  templateUrl: './station.component.html',
  styleUrls: ['./station.component.css']
})
export class StationComponent implements OnInit, OnDestroy {
  @Input() id: number = -1;
  stationName: string = "";
  stationAddress: string = "";
  isDispenserLoading = false;
  isTankLoading = false;
  isLogLoading = false;
  dispenserList: DispenserRecord[] = [];
  dispenserSocket: { [key: string]: WebSocketSubject<WSDispenserRecord> } = {}
  tankSocket: { [key: string]: WebSocketSubject<WSTankRecord> } = {}
  logSocket:  { [key: string]: WebSocketSubject<WSLogRecord> } = {}
  logsocket: WebSocketSubject<LogRecord[]> | undefined
  sumRevenueByLogTypeSocket: WebSocketSubject<sumRevenueByLogType[]> | undefined
  sumRevenueByFuelNameSocket: WebSocketSubject<sumRevenueByName[]> | undefined
  tankList: TankRecord[] = [];
  logList: LogRecord[] = [];
  _temp_statusList: number[] = [];
  public pieChartType: any = 'pie';
  public pieChartOptions: ChartConfiguration<'pie'>['options'] = {
    responsive: true,
    animation: false,
    plugins: {
      legend: {
        display: true,
        position: 'bottom' as const,
      }
    }
  };
  chartLabels: string[] = [];
  chartDataAccount: number[] = [];
  chartDataFuel: number[] = [];
  revenueChartData: any;
  fuelChartData: any;

  constructor(
    private http: HttpClient,
    private titleServer: TitleService,
    private route: ActivatedRoute,
    private TitleService: TitleService
  ) { }

  ngOnInit(): void {

    const snapshot = this.route.snapshot;
    this.stationName = snapshot.queryParams['name'];
    this.stationAddress = snapshot.queryParams['address'];
    setTimeout(() => {
      this.titleServer.updateTitle(this.stationName)
    }, 0);

    // ✅ Load sum revenue by fuel name
    this.sumRevenueByFuelNameSocket = webSocket<sumRevenueByName[]>(environment.wsServerURI + `/ws/shift/name/${this.id}`)
    this.sumRevenueByFuelNameSocket.subscribe({
      next: res => {
        console.log("Received data:", res);
        this.chartLabels = res.map(item => item.FuelName);
        this.chartDataFuel = res.map(item => item.TotalLiters);
        this.fuelChartData = {
          labels: this.chartLabels,
          datasets: [{
            data: this.chartDataFuel,
            backgroundColor: [
              '#FF6384',
              '#36A2EB',
              '#FFCE56',
              '#4BC0C0',
              '#9966FF'
            ]
          }]
        };
      }
    })

    this.sumRevenueByLogTypeSocket = webSocket<sumRevenueByLogType[]>(environment.wsServerURI + `/ws/shift/type/${this.id}`)
    this.sumRevenueByLogTypeSocket.subscribe({
      next: res => {
        this.chartLabels = res.map(item => item.LogTypeName);
        this.chartDataAccount = res.map(item => item.TotalAmount);
        this.chartDataFuel = res.map(item => item.TotalLiters);
        this.revenueChartData = {
          labels: this.chartLabels,
          datasets: [{
            data: this.chartDataAccount,
            backgroundColor: [
              '#FF6384',
              '#36A2EB',
              '#FFCE56',
              '#4BC0C0',
              '#9966FF'
            ]
          }]
        };
      }
    })

    this.logsocket = webSocket<LogRecord[]>(environment.wsServerURI + `/ws/log/station/${this.id}`);
    this.logsocket.subscribe({
      next: res => {
        this.logList = res; 
        console.table(res);
        this.logList.forEach((value, index) => {
          this.logSocket[value.StationId] = webSocket<WSLogRecord>(environment.wsServerURI + `/ws/log/station/${this.id}?token=${localStorage.getItem('jwt')}`)
          this.logSocket[value.StationId].subscribe({
            next: (Datares: WSLogRecord) => {
              res[index].Name = Datares.name
              res[index].FuelName = Datares.fuelName
              res[index].TotalLiters = Datares.totalLiters
              res[index].Price = Datares.price
              res[index].TotalAmount = Datares.totalAmount
              res[index].Time = Datares.time
            },
            error: (err) => {
              console.error(`Error at station ${value.StationId}: ${err}`);
            }
          })

        })
      },
      error: err => console.error("(WebSocket error) - not load data log", err),
    });


    forkJoin({
      dispenser: this.http.get(environment.serverURI + `/dispenser/station/${this.id}`, { observe: "response" }),
      tank: this.http.get(environment.serverURI + `/tank/station/${this.id}`, { observe: "response" }),
    }).
      pipe(
        mergeMap((res) => of(res).pipe(delay(1000))), //Simulating delay
        catchError((err) => of(err).pipe(delay(1000), mergeMap(() => throwError(() => err)))), //Simulating delay
        finalize(() => {
          this.isDispenserLoading = false
          this.isTankLoading = false
          this.isLogLoading = false
        })
      ).subscribe({
        next: (res: {
          dispenser: HttpResponse<any>,
          tank: HttpResponse<any>,

        }) => {
          this.dispenserList = res.dispenser.body
          this.dispenserList.forEach((value, index) => {
            this.dispenserSocket[value.dispenserId] = webSocket<WSDispenserRecord>(environment.wsServerURI + `/ws/dispenser/${value.dispenserId}?token=${localStorage.getItem('jwt')}`)
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
          this.tankList = res.tank.body
          this.tankList.forEach((value, index) => {
            this.tankSocket[value.tankId] = webSocket<WSTankRecord>(environment.wsServerURI + `/ws/tank/${value.tankId}?token=${localStorage.getItem('jwt')}`)
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
          // this.logList = res.log  
          // this.logList.forEach((value, index) => {
          //   this.logSocket[value.stationId] = webSocket<WSLogRecord>(environment.wsServerURI + `/ws/log/station/${value.stationId}?token=${localStorage.getItem('jwt')}`)
          //   this.logSocket[value.stationId].subscribe({
          //     next: (res: WSLogRecord) => {
          //       this.logList[index].name = res.name;
          //       this.logList[index].fuelName=res.fuelName;
          //       this.logList[index].totalLiters=res.totalLiters;
          //       this.logList[index].price=res.price;
          //       this.logList[index].totalAmount=res.totalAmount;
          //       this.logList[index].time=res.time;
          //     },
          //     error: (err) => {
          //       console.error(`Error at tank ${value.stationId}: ${err}`);
          //     }
          //   })
          // })   
        },
        error: (err: HttpErrorResponse) => {
          console.error(err.message);
        }
      })
    window.onbeforeunload = () => this.ngOnDestroy()
  }

  ngOnDestroy(): void {
    for (const key in this.dispenserSocket) {
      this.dispenserSocket[key].complete()
    }
    for (const key in this.tankSocket) {
      this.tankSocket[key].complete()
    }
    this.sumRevenueByLogTypeSocket?.complete()
    this.sumRevenueByFuelNameSocket?.complete()
  }
}

