declare let BigNumber: any;
import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit, OnDestroy } from '@angular/core';
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
  imports: [NgChartsModule,
    CommonModule, FormsModule],
  templateUrl: './station.component.html',
  styleUrls: ['./station.component.css']
})
export class StationComponent implements OnInit, OnDestroy {
  @Input() id: number = -1;
  showLogs = true;
  stationName: string = "";
  stationAddress: string = "";
  isDispenserLoading = false;
  isTankLoading = false;

  dispenserSocket: { [key: string]: WebSocketSubject<WSDispenserRecord[]> } = {}
  dispensersocket: WebSocketSubject<DispenserRecord[]> | undefined
  dispenserList: DispenserRecord[] = [];
  Status: (string | undefined)[] = [];
  Liter: (number | undefined)[] = [];
  LongName: string[] = [];
  ShortName: string[] = [];
  TotalPrice: (number | undefined)[] = [];

  tankList: TankRecord[] = [];
  tankSocket: { [key: string]: WebSocketSubject<WSTankRecord[]> } = {}
  tanksocket: WebSocketSubject<TankRecord[]> | undefined
  TankName: number[]=[];
  TankShortName: string[]=[];
  Percentage: string[]=[];

  logSocket: { [key: string]: WebSocketSubject<WSLogRecord[]> } = {}
  logsocket: WebSocketSubject<LogRecord[]> | undefined
  logList: LogRecord[] = [];
  DispenserId: number[] = [];
  DispenserName: number[] = [];
  FuelName: string[] = [];
  TotalLiters: number[] = [];
  Price: number[] = [];
  TotalAmount: number[] = [];

  sumRevenueByLogTypeSocket: WebSocketSubject<sumRevenueByLogType[]> | undefined
  sumRevenueByFuelNameSocket: WebSocketSubject<sumRevenueByName[]> | undefined
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
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    const snapshot = this.route.snapshot;
    this.stationName = snapshot.queryParams['name'];
    this.stationAddress = snapshot.queryParams['address'];
    // ✅ Load sum revenue by fuel name
    this.sumRevenueByFuelNameSocket = webSocket<sumRevenueByName[]>(environment.wsServerURI + `/ws/shift/name/${this.id}?token=${localStorage.getItem('jwt')}`)
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

    this.sumRevenueByLogTypeSocket = webSocket<sumRevenueByLogType[]>(environment.wsServerURI + `/ws/shift/type/${this.id}?token=${localStorage.getItem('jwt')}`)
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

    // ✅ Load table log by StationId
    this.logsocket = webSocket<WSLogRecord[]>(`${environment.wsServerURI}/ws/log/station/${this.id}?token=${localStorage.getItem('jwt')}`);
    this.logsocket.subscribe({
      next: (res: WSLogRecord[]) => {
        this.logList = res;
        console.log("load log data: ", this.logList);
        this.showLogs = true;
        this.DispenserName = this.logList.map((item) => item.Name);
        this.FuelName = this.logList.map((item) => item.FuelName);
        this.TotalLiters = this.logList.map((item) => item.TotalLiters);
        this.Price = this.logList.map((item) => item.Price);
        this.TotalAmount = this.logList.map((item) => item.TotalAmount);
      },
      error: err => {
        console.error("(WebSocket error) - not load data log", err);
      }
    });


    forkJoin({
      dispenser: this.http.get(environment.serverURI + `/dispenser/station/${this.id}`, { observe: "response", withCredentials: false }),
      tank: this.http.get(environment.serverURI + `/tank/station/${this.id}`, { observe: "response", withCredentials: false }),
    }).
    pipe(
      mergeMap((res) => of(res).pipe(delay(1000))), //Simulating delay
      catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))), //Simulating delay
      finalize(() => {
        this.isDispenserLoading = false
        this.isTankLoading = false
      })
    ).subscribe({
      next: (res: {
        dispenser: HttpResponse<any>,
        tank: HttpResponse<any>,
      }) => {
        this.dispenserList = res.dispenser.body
        console.log("dispenser data", this.dispenserList)
        this.dispenserList.forEach((value, index) => {
          this.dispenserSocket[value.dispenserId] = webSocket<WSDispenserRecord[]>(environment.wsServerURI + `/ws/dispenser/${this.id}?token=${localStorage.getItem('jwt')}`)
          this.dispenserSocket[value.dispenserId].subscribe({
            next: (res: WSDispenserRecord[]) => {
              this.dispenserList[index].liter = res[index]?.liter
              this.dispenserList[index].totalAmount = res[index]?.totalAmount
              this.dispenserList[index].status = res[index]?.status
            },
            error: (err) => {
              console.error(`Error at dispenser ${value.dispenserId}: ${err}`);
            }
          })
        })
        this.tankList = res.tank.body
        console.log("tank data: ", this.tankList)
        this.tankList.forEach((value, index) => {
          this.tankSocket[value.tankId] = webSocket<WSTankRecord[]>(environment.wsServerURI + `/ws/tank/station/${this.id}?token=${localStorage.getItem('jwt')}`)
          this.tankSocket[value.tankId].subscribe({
            next: (res: WSTankRecord[]) => {
              this.tankList[index].currentVolume = res[index]?.currentVolume
              const vMax = new BigNumber(this.tankList[index].maxVolume)
              this.tankList[index].percentage = new BigNumber(res[index]?.currentVolume).dividedBy(vMax).times(100).toFixed(2).toString()
            },
            error: (err) => {
              console.error(`Error at tank ${value.tankId}: ${err}`);
            }
          })
        })
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
