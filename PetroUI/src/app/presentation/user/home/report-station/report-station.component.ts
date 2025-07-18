import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgChartsModule } from 'ng2-charts';
import { FormControl, ReactiveFormsModule, UntypedFormGroup, Validators } from '@angular/forms';
import { WebSocketService } from './../../../../services/web-socket.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { delay, mergeMap, catchError, finalize, of, throwError, forkJoin } from 'rxjs';
import { CommonModule } from '@angular/common';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket'
import { revenuestation, WSrevenuestation } from './model/sumrevenuestation-record';
import { revenuestationday, WSrevenuestationday } from './model/sumrevenueday-record';
import { ChartDataset, ChartOptions } from 'chart.js';

@Component({
    standalone: true,
    selector: 'app-report-station',
    templateUrl: './report-station.component.html',
    imports: [NgChartsModule, ReactiveFormsModule, CommonModule]
})

export class ReportStationComponent implements OnInit, OnDestroy {
    loading = true;
    @Input() id: number = -1;
    isDispenserLoading = false;
    isTankLoading = false;
    isLogLoading = false;
    revstationSocket: { [key: string]: WebSocketSubject<WSrevenuestation> } = {}
    revenuesocket: WebSocketSubject<revenuestation[]> | undefined;
    revstation: revenuestation[] = [];
    DataAccount: number[] = [];
    DataLitters: number[] = [];
    DataProfit: number[] = [];
    TotalLitters = 0;
    TotalRevenue = 0;
    totalProfit = 0;
    revstationDaySocket: { [key: string]: WebSocketSubject<WSrevenuestationday> } = {}
    revenuedaysocket: WebSocketSubject<revenuestationday[]> | undefined;
    revstationday: revenuestationday[] = [];
    Day: string[] = [];
    DayAccount: number[] = [];
    DayProfit: number[] = [];

    public barChartOptions: ChartOptions<'bar'> = {
        responsive: false,
        animation: {
            duration: 0
        },
        plugins: {
            legend: { display: true },
            tooltip: {
                enabled: false
            }
        },
    };
    public barChartData: {
        labels: string[],
        datasets: ChartDataset<'bar'>[]
    } = {
            labels: ['Doanh thu', 'Lợi nhuận'],
            datasets: [
                { data: [0, 0], label: 'VNĐ' }
            ]
        };

    constructor(
        private router: Router,
        private wsService: WebSocketService,
        private http: HttpClient
    ) { }

    ngOnInit(): void {
        this.revenuesocket = webSocket<revenuestation[]>(environment.wsServerURI + `/ws/station/${this.id}`)
        this.revenuesocket.subscribe({
            next: res => {
                this.revstation = res;
                this.revstation.forEach((value, index) => {
                    this.revstationSocket[value.StationId] = webSocket<WSrevenuestation>(environment.wsServerURI + `/ws/station/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.revstationSocket[value.StationId].subscribe({
                        next: (Datares: WSrevenuestation) => {
                            this.revstation[index].TotalLiters = Datares.liter
                            this.revstation[index].TotalRevenue = Datares.revenue
                            this.revstation[index].TotalProfit = Datares.profit
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.StationId}: ${err}`);
                        }
                    })
                })
                this.DataAccount = this.revstation.map((item) => item.TotalRevenue);
                this.TotalRevenue = this.DataAccount.reduce((acc, val) => acc + val, 0);
                this.DataLitters = this.revstation.map((item) => item.TotalLiters);
                this.TotalLitters = this.DataLitters.reduce((acc, val) => acc + val, 0);
                this.DataProfit = this.revstation.map((item) => item.TotalProfit);
                this.totalProfit = this.DataProfit.reduce((acc, val) => acc + val, 0);
            },
            error: err => {
                console.error(err);
            }
        })

        this.revenuedaysocket = webSocket<revenuestationday[]>(environment.wsServerURI + `/ws/station/sumrevenueday/${this.id}`)
        this.revenuedaysocket.subscribe({
            next: res => {
                this.revstationday = res
                console.log('Bar line Chart Websocket connected');
                console.log("bar line chart data: ", res)
                this.revstationday.forEach((value, index) => {
                    this.revstationDaySocket[value.StationId] = webSocket<WSrevenuestationday>(environment.wsServerURI + `/ws/station/sumrevenueday/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.revstationDaySocket[value.StationId].subscribe({
                        next: (Datares: WSrevenuestationday) => {
                            res[index].StationName = Datares.name
                            res[index].Date = Datares.date
                            res[index].TotalRevenue = Datares.revenue
                            res[index].TotalProfit = Datares.profit
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.StationId}: ${err}`);
                        }
                    })

                })
                const dateSet = new Set<string>();
                res.forEach(item => dateSet.add(item.Date));
                const sortedDates = Array.from(dateSet).sort();
                this.Day = sortedDates.map(date => new Date(date).toLocaleDateString('vi-VN'));
                this.DayAccount = this.revstationday.map((item) => item.TotalRevenue);
                this.DayProfit = this.revstationday.map((item) => item.TotalProfit);
                this.barChartData = {
                    labels: this.Day,
                    datasets: [
                        {
                            label: 'Doanh thu (VNĐ)',
                            data: this.DayAccount,
                            backgroundColor: '#42A5F5'
                        },
                        {
                            label: 'Lợi nhuận (VNĐ)',
                            data: this.DayProfit,
                            backgroundColor: '#66BB6A'
                        }
                    ]
                };
            },
            error: err => {
                console.error(err);
            }
        })
    }

    ngOnDestroy(): void {
        this.revenuesocket?.complete()
        for (const socket in this.revstationSocket, this.revstationDaySocket) {
            this.revstationSocket[socket].complete()
            this.revstationDaySocket[socket].complete()
        }
    }
}
