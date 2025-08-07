import { HttpClient } from '@angular/common/http';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ChartOptions } from 'chart.js';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { environment } from '../../../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { revenuetypeyear, yearrevenuefuel } from './year-revenue-record';
@Component({
    selector: 'app-report-station-chart',
    standalone: true,
    imports: [ReactiveFormsModule, NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule],
    templateUrl: './year-dialog.component.html',
    //   styleUrl: './report-station.component.css'
})

export class ReportYearComponent implements OnChanges {
    isOpenErrorModal = false
    @Input() id: number = -1;
    @Input() year: string = '';
    stationName: string[] = [];
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
    piechartfuelyearsocket: WebSocketSubject<yearrevenuefuel[]> | undefined;
    revfuelyear: yearrevenuefuel[] = [];
    YearFuelName: string[] = [];
    Yearpie: string[] = [];
    YearTotalLiters: number[] = [];
    pieChartFuelYearData: any = {};

    piecharttypeyearsocket: WebSocketSubject<revenuetypeyear[]> | undefined;
    revtypeyear: revenuetypeyear[] = [];
    YearTypeName: string[] = [];
    Yeartypepie: string[] = [];
    YearTotalAmount: number[] = [];
    pieChartTypeYearData: any = {};

    constructor(private router: ActivatedRoute, private http: HttpClient) { }
    ngOnInit(): void {
        console.log("Year:", this.year);
        this.id = this.router.parent?.snapshot.params['id'] ?? '';
        this.piechartfuelyearsocket = webSocket<yearrevenuefuel[]>(environment.wsServerURI + `/ws/year/name/${this.id}?token=${localStorage.getItem('jwt')}`);
        this.piechartfuelyearsocket.subscribe({
            next: res => {
                this.revfuelyear = res
                console.log('Pie Chart year Websocket connected');
                console.log("✔️ year data: ", res)
                this.YearFuelName = this.revfuelyear.map((item) => item.FuelName);
                this.Yearpie = this.revfuelyear.map((item) => item.Year);
                this.YearTotalLiters = this.revfuelyear.map((item) => item.TotalLiters);
                this.pieChartFuelYearData = {
                    labels: this.YearFuelName,
                    datasets: [{
                        data: this.YearTotalLiters,
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

        this.piecharttypeyearsocket = webSocket<revenuetypeyear[]>(environment.wsServerURI + `/ws/sumrenuetype/getyear/${this.id}/${this.year}`);
        this.piecharttypeyearsocket.subscribe({
            next: res => {
                this.revtypeyear = res
                console.log('Pie Chart year type Websocket connected');
                console.log("year type data: ", res)
                this.YearTypeName = this.revtypeyear.map((item) => item.LogTypeName);
                this.YearTotalAmount = this.revtypeyear.map((item) => item.TotalAmount);
                this.pieChartTypeYearData = {
                    labels: this.YearTypeName,
                    datasets: [{
                        data: this.YearTotalAmount,
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
        this.piechartfuelyearsocket?.complete;
        this.piecharttypeyearsocket?.complete;
    }
}
