import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, input, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ChartOptions } from 'chart.js';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { environment } from '../../../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { monthrevenuefuel, revenuetypemonth, WSmonthrevenuefuel, WSrevenuetypemonth } from './month-revenue-record';
@Component({
    selector: 'app-report-station-chart',
    standalone: true,
    imports: [ReactiveFormsModule, NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule],
    templateUrl: './month-dialog.component.html',
    //   styleUrl: './report-station.component.css'
})

export class ReportMonthComponent implements OnChanges {
    isOpenErrorModal = false
    @Input() id: number = -1;
    @Input() month: string = '';
    isUpdateLoading = false;

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
    piechartfuelmonthSocket: { [key: string]: WebSocketSubject<WSmonthrevenuefuel> } = {}
    piechartfuelmonthsocket: WebSocketSubject<monthrevenuefuel[]> | undefined;
    revfuelmonth: monthrevenuefuel[] = [];
    MonthFuelName: string[] = [];
    Monthpie: string[] = [];
    MonthTotalLiters: number[] = [];
    pieChartFuelMonthData: any = {};

    piecharttypemonthSocket: { [key: string]: WebSocketSubject<WSrevenuetypemonth[]> } = {}
    piecharttypemonthsocket: WebSocketSubject<revenuetypemonth[]> | undefined;
    revtypemonth: revenuetypemonth[] = [];
    MonthTypeName: string[] = [];
    Monthtypepie: string[] = [];
    MonthTotalAmount: number[] = [];
    pieChartTypeMonthData: any = {};

    constructor(private router: ActivatedRoute, private http: HttpClient) { }
    ngOnInit(): void {
        const [mm, yy] = this.month.split("-");
        console.log("Tháng:", mm, "-", yy);
        this.id = this.router.parent?.snapshot.params['id'] ?? '';
        this.piechartfuelmonthsocket = webSocket<monthrevenuefuel[]>(environment.wsServerURI + `/ws/sumrenuename/getmonth/${this.id}/${mm}/${yy}`);
        this.piechartfuelmonthsocket.subscribe({
            next: res => {
                this.revfuelmonth = res
                console.log('Pie Chart month Websocket connected');
                console.log("✔️ month data: ", res)
                this.revfuelmonth.forEach((value, index) => {
                    this.piechartfuelmonthSocket[value.StationId] = webSocket<WSmonthrevenuefuel>(environment.wsServerURI + `/ws/sumrenuename/getmonth/${this.id}/${mm}/${yy}?token=${localStorage.getItem('jwt')}`)
                    this.piechartfuelmonthSocket[value.StationId].subscribe({
                        next: (Datares: WSmonthrevenuefuel) => {
                            res[index].FuelName = Datares.FuelName
                            res[index].Month = Datares.Month
                            res[index].TotalAmount = Datares.TotalAmount
                            res[index].TotalLiters = Datares.TotalLiters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.StationId}: ${err}`);
                        }
                    })
                })
                this.MonthFuelName = this.revfuelmonth.map((item) => item.FuelName);
                this.Monthpie = this.revfuelmonth.map((item) => item.Month);
                this.MonthTotalLiters = this.revfuelmonth.map((item) => item.TotalLiters);
                this.pieChartFuelMonthData = {
                    labels: this.MonthFuelName,
                    datasets: [{
                        data: this.MonthTotalLiters,
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

        this.piecharttypemonthsocket = webSocket<revenuetypemonth[]>(environment.wsServerURI + `/ws/sumrenuetype/getmonth/${this.id}/${mm}/${yy}`);
        this.piecharttypemonthsocket.subscribe({
            next: res => {
                this.revtypemonth = res
                console.log('✔️ Pie Chart month type Websocket connected');
                console.log("month type data: ", res)
                this.revtypemonth.forEach((value, index) => {
                    this.piecharttypemonthSocket[value.StationId] = webSocket<WSrevenuetypemonth[]>(environment.wsServerURI + `/ws/sumrenuetype/getmonth/${this.id}/${mm}/${yy}?token=${localStorage.getItem('jwt')}`)
                    this.piecharttypemonthSocket[value.StationId].subscribe({
                        next: (Datares: WSrevenuetypemonth[]) => {
                            res[index].LogTypeName = Datares[index].LogTypeName
                            res[index].TotalAmount = Datares[index].TotalAmount
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.StationId}: ${err}`);
                        }
                    })
                })
                this.MonthTypeName = this.revtypemonth.map((item) => item.LogTypeName);
                this.MonthTotalAmount = this.revtypemonth.map((item) => item.TotalAmount);
                this.pieChartTypeMonthData = {
                    labels: this.MonthTypeName,
                    datasets: [{
                        data: this.MonthTotalAmount,
                        backgroundColor: [
                            '#FF6384',
                            '#36A2EB',
                            '#FFCE56',
                            '#4BC0C0',
                            '#9966FF'
                        ]
                    }]
                };
                console.log("CharttypeData:", this.pieChartTypeMonthData);
            },
            error: err => {
                console.error(err);
            }
        })


    }
    ngOnChanges(changes: SimpleChanges): void {

    }
}
