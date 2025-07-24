import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ChartOptions } from 'chart.js';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { environment } from '../../../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { revenuefuel, revenuetypeday, WSrevenuefuel, WSrevenuetypeday } from './revenue-record';
@Component({
  selector: 'app-report-station-chart',
  standalone: true,
  imports: [ReactiveFormsModule, NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './date-dialog.component.html',
  //   styleUrl: './report-station.component.css'
})

export class ReportComponent implements OnChanges {
  isOpenErrorModal = false
  @Input() id: number = -1;
  @Input() date: string = '';
  isUpdateLoading = false;
  stationName: string = '';

  public PieChartFuelOption: ChartOptions<'pie'> = {
    responsive: false,
    animation: false,
    plugins: {
      legend: {
        display: true,
        position: 'bottom' as const,
      }
    }
  };
  piechartfueldaySocket: { [key: string]: WebSocketSubject<WSrevenuefuel[]> } = {}
  piechartfueldaysocket: WebSocketSubject<revenuefuel[]> | undefined;
  revfuelday: revenuefuel[] = [];
  DayFuelName: string[] = [];
  Date: string[] = [];
  DayTotalLiters: number[] = [];
  pieChartFuelDateData: any = {};

  piecharttypedaySocket: { [key: string]: WebSocketSubject<WSrevenuetypeday[]> } = {}
  piecharttypedaysocket: WebSocketSubject<revenuetypeday[]> | undefined;
  revtypeday: revenuetypeday[] = [];
  DayTypeName: string[] = [];
  DateType: string[] = [];
  DayTotalAmount: number[] = [];
  pieChartTypeDateData: any = {};
  constructor(private router: ActivatedRoute, private http: HttpClient) { }
  // datetime = '2025-06-06';
  ngOnInit(): void {
    this.date = this.router.snapshot.params['date'];
    this.id = this.router.parent?.snapshot.params['id'] ?? '';
    console.log("date: ", this.date, "- ", this.id);
    this.piechartfueldaysocket = webSocket<revenuefuel[]>(environment.wsServerURI + `/ws/sumrenuename/getdate/${this.id}/${this.date}`);
    this.piechartfueldaysocket.subscribe({
      next: res => {
        this.revfuelday = res;
        console.log('==> Pie Chart Websocket connected');
        console.log('==> data name data:', res);
        res.forEach((value, index) => {
          const key = `${value.StationId}-${value.Time}`;
          this.piechartfueldaySocket[key] = webSocket<WSrevenuefuel[]>(environment.wsServerURI + `/ws/sumrenuename/getdate/${this.id}/${this.date}?token=${localStorage.getItem('jwt')}`);
          this.piechartfueldaySocket[key].subscribe({
            next: (Datares: WSrevenuefuel[]) => {
              res[index].Time = Datares[index].Time;
              res[index].TotalAmount = Datares[index].TotalAmount;
              res[index].TotalLiters = Datares[index].TotalLiters;
            },
            error: (err) => {
              console.error(`Error WebSocket stationId ${value.StationId}:`, err);
            }
          });
        });
        this.DayFuelName = this.revfuelday.map((item) => item.FuelName);
        this.DayTotalLiters = this.revfuelday.map((item) => item.TotalLiters);
        this.pieChartFuelDateData = {
          labels: this.DayFuelName,
          datasets: [{
            data: this.DayTotalLiters,
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF']
          }]
        };
      },
      error: err => {
        console.error('Error WebSocket:', err);
      }
    });

    this.piecharttypedaysocket = webSocket<revenuetypeday[]>(environment.wsServerURI + `/ws/sumrenuetype/getdate/${this.id}/${this.date}`);
    this.piecharttypedaysocket.subscribe({
      next: res => {
        this.revtypeday = res
        console.log('Pie Chart date type Websocket connected');
        console.log("date type data: ", res)
        this.revtypeday.forEach((value, index) => {
          this.piecharttypedaySocket[value.StationId] = webSocket<WSrevenuetypeday[]>(environment.wsServerURI + `/ws/sumrenuetype/getdate/${this.id}/${this.date}?token=${localStorage.getItem('jwt')}`)
          this.piecharttypedaySocket[value.StationId].subscribe({
            next: (Datares: WSrevenuetypeday[]) => {
              res[index].LogTypeName = Datares[index].LogTypeName
              // res[index].Date = Datares[index].Date
              res[index].TotalAmount = Datares[index].TotalAmount
            },
            error: (err) => {
              console.error(`Error at station ${value.StationId}: ${err}`);
            }
          })

        })
        this.DayTypeName = this.revtypeday.map((item) => item.LogTypeName);
        this.Date = this.revtypeday.map((item) => item.Date);
        this.DayTotalAmount = this.revtypeday.map((item) => item.TotalAmount);
        this.pieChartTypeDateData = {
          labels: this.DayTypeName,
          datasets: [{
            data: this.DayTotalAmount,
            backgroundColor: [
              '#FF6384',
              '#36A2EB',
              '#FFCE56',
              '#4BC0C0',
              '#9966FF'
            ]
          }]
        };
      },
      error: err => {
        console.error(err);
      }
    })

  }
  ngOnChanges(changes: SimpleChanges): void {

  }
}
