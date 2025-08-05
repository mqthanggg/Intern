import { HttpClient } from '@angular/common/http';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ChartOptions } from 'chart.js';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { environment } from '../../../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { revenuefuel, revenuetypeday } from './revenue-record';

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
  piechartfueldaysocket: WebSocketSubject<revenuefuel[]> | undefined;
  revfuelday: revenuefuel[] = [];
  DayFuelName: string[] = [];
  Date: string[] = [];
  DayTotalLiters: number[] = [];
  pieChartFuelDateData: any = {};

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

    this.piechartfueldaysocket = webSocket<revenuefuel[]>(environment.wsServerURI + `/ws/sumrenuename/getdate/${this.id}/${this.date}?token=${localStorage.getItem('jwt')}`);
    this.piechartfueldaysocket.subscribe({
      next: res => {
        this.revfuelday = res;
        console.log('==> Pie Chart Websocket connected');
        console.log('==> data name data:', res);
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

    this.piecharttypedaysocket = webSocket<revenuetypeday[]>(environment.wsServerURI + `/ws/sumrenuetype/getdate/${this.id}/${this.date}?token=${localStorage.getItem('jwt')}`);
    this.piecharttypedaysocket.subscribe({
      next: res => {
        this.revtypeday = res
        console.log('Pie Chart date type Websocket connected');
        console.log("date type data: ", res);
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
  
  ngOnChanges(): void {
    this.piechartfueldaysocket?.complete;
    this.piecharttypedaysocket?.complete;
  }
}
