import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NgChartsModule } from 'ng2-charts';
import { ReactiveFormsModule } from '@angular/forms';
import { environment } from '../../../../../environments/environment';
import { CommonModule } from '@angular/common';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket'
import { revenuefuelday, revenuefuelmonth, revenuefuelyear, revenuestation, revenuestationday, revenuestationmonth, revenuestationyear, revenuetypeday, revenuetypemonth, revenuetypeyear, WSrevenuefuelday, WSrevenuefuelmonth, WSrevenuefuelyear, WSrevenuestation, WSrevenuestationday, WSrevenuestationmonth, WSrevenuestationyear, WSrevenuetypeday, WSrevenuetypemonth, WSrevenuetypeyear } from './model/sumrevenuestation-record';
import { ChartDataset, ChartOptions } from 'chart.js';
import { FormsModule } from '@angular/forms';
@Component({
    standalone: true,
    selector: 'app-report-station',
    templateUrl: './report-station.component.html',
    imports: [NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule, RouterOutlet]
})

export class ReportStationComponent implements OnInit, OnDestroy {
    @Input() id: number = -1;
    stationName: string [] = [];
    revstationSocket: { [key: string]: WebSocketSubject<WSrevenuestation[]> } = {}
    revenuesocket: WebSocketSubject<revenuestation[]> | undefined;
    revstation: revenuestation[] = [];
    DataAccount: number[] = [];
    DataLitters: number[] = [];
    DataProfit: number[] = [];
    TotalLitters = 0;
    TotalRevenue = 0;
    totalProfit = 0;
    // Load Bar chart  
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
    revstationDaySocket: { [key: string]: WebSocketSubject<WSrevenuestationday[]> } = {}
    revenuedaysocket: WebSocketSubject<revenuestationday[]> | undefined;
    revstationday: revenuestationday[] = [];
    StationId: number[] = [];
    Day: string[] = [];
    DayAccount: number[] = [];
    DayProfit: number[] = [];

    revenuemonthsocket: WebSocketSubject<revenuestationmonth[]> | undefined;
    revstationmonth: revenuestationmonth[] = [];
    Month: string[] = [];
    MonthAccount: number[] = [];
    MonthProfit: number[] = [];

    revenueyearsocket: WebSocketSubject<revenuestationyear[]> | undefined;
    revstationyear: revenuestationyear[] = [];
    Year: string[] = [];
    YearAccount: number[] = [];
    YearProfit: number[] = [];

    dropdownOpen = false;
    selectedChart: 'day' | 'month' | 'year' = 'day';
    onChartTypeChange() {
        switch (this.selectedChart) {
            case 'day':
                this.loadBarChartDay();
                break;
            case 'month':
                this.loadBarChartMonth();
                break;
            case 'year':
                this.loadBarChartYear();
                break;
        }
    }
    toggleDropdown() {
        this.dropdownOpen = !this.dropdownOpen;
    }
    constructor(private router: Router) { }
    title: string = "";
    ngOnInit(): void {
        this.revenuesocket = webSocket<revenuestation[]>(environment.wsServerURI + `/ws/station/${this.id}?token=${localStorage.getItem('jwt')}`)
        this.revenuesocket.subscribe({
            next: res => {
                this.revstation = res;
                console.log("✔️ total revenue station: ", res);
                this.revstation.forEach((value, index) => {
                    this.revstationSocket[value.StationId] = webSocket<WSrevenuestation[]>(environment.wsServerURI + `/ws/station/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.revstationSocket[value.StationId].subscribe({
                        next: (Datares: WSrevenuestation[]) => {
                            this.revstation[index].TotalLiters = Datares[index].TotalLiters
                            this.revstation[index].TotalRevenue = Datares[index].TotalRevenue
                            this.revstation[index].TotalProfit = Datares[index].TotalProfit
                        },
                    })
                })
                this.DataAccount = this.revstation.map((item) => item.TotalRevenue);
                this.TotalRevenue = this.DataAccount.reduce((acc, val) => acc + val, 0);
                this.DataLitters = this.revstation.map((item) => item.TotalLiters);
                this.TotalLitters = this.DataLitters.reduce((acc, val) => acc + val, 0);
                this.DataProfit = this.revstation.map((item) => item.TotalProfit);
                this.totalProfit = this.DataProfit.reduce((acc, val) => acc + val, 0);
                this.stationName=this.revstation.map((item) => item.StationName);
            },
            // error: (err) => console.error('WebSocket error:', err),
            // complete: () => console.warn('WebSocket connection closed'),
        })
        this.loadBarChartDay();
    }

    loadBarChartDay() {
        this.revenuedaysocket = webSocket<revenuestationday[]>(environment.wsServerURI + `/ws/station/revenueday/${this.id}?token=${localStorage.getItem('jwt')}`)
        this.revenuedaysocket.subscribe({
            next: res => {
                this.revstationday = res
                console.log('✔️ Bar Chart date Websocket connected');
                console.log("date data: ", res)
                const dateSet = new Set<string>();
                res.forEach(item => dateSet.add(item.Date));
                const sortedDates = Array.from(dateSet).sort();
                this.StationId = this.revstationday.map((item) => item.StationId);
                this.Day = sortedDates.map(date => {
                    const d = new Date(date);
                    if (!isNaN(d.getTime())) {
                        const year = d.getFullYear();
                        const month = String(d.getMonth() + 1).padStart(2, '0');
                        const day = String(d.getDate()).padStart(2, '0');
                        return `${year}-${month}-${day}`;  // chuẩn ISO: yyyy-MM-dd
                    }
                    return ''; // hoặc xử lý lỗi
                });
                // this.Day = this.revstationday.map((item) =>item.Date)
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
    onDayClick(event: any): void {
        const activePoints = event.active;
        if (activePoints && activePoints.length > 0) {
            const chartElement = activePoints[0];
            const chartIndex = chartElement.index;
            const clickedStationId = this.StationId[chartIndex];
            const clickedDate = this.Day[chartIndex];
            const clickedRevenue = this.DayAccount[chartIndex];
            const clickedProfit = this.DayProfit[chartIndex];
            console.log('Station Id: ', clickedStationId);
            console.log('🟡 Select day:', clickedDate);
            console.log('➡️ total revenue:', clickedRevenue);
            console.log('➡️ total profit', clickedProfit);

            this.router.navigate(['user/home/report', clickedStationId,'day', clickedDate]);
        }
    }
    loadBarChartMonth() {
        this.revenuemonthsocket = webSocket<revenuestationmonth[]>(environment.wsServerURI + `/ws/station/revenuemonth/${this.id}`)
        this.revenuemonthsocket.subscribe({
            next: res => {
                this.revstationmonth = res
                console.log('-- Bar Chart month Websocket connected');
                console.log("month data: ", res)
                this.Month = this.revstationmonth.map((item) => item.Month);
                this.MonthAccount = this.revstationmonth.map((item) => item.TotalRevenue);
                this.MonthProfit = this.revstationmonth.map((item) => item.TotalProfit);
                console.log("dt: ", this.MonthAccount);
                this.barChartData = {
                    labels: this.Month,
                    datasets: [
                        {
                            label: 'Doanh thu (VNĐ)',
                            data: this.MonthAccount,
                            backgroundColor: '#42A5F5'
                        },
                        {
                            label: 'Lợi nhuận (VNĐ)',
                            data: this.MonthProfit,
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
    onMonthClick(event: any): void {
        const activePoints = event.active;
        if (activePoints && activePoints.length > 0) {
            const chartElement = activePoints[0];
            const chartIndex = chartElement.index;
            const clickedStationId = this.StationId[chartIndex];
            const clickedMonth = this.Month[chartIndex];
            const clickedRevenue = this.MonthAccount[chartIndex];
            const clickedProfit = this.MonthProfit[chartIndex];
            console.log('Station Id: ', clickedStationId);
            console.log('🟡 Select month:', clickedMonth);
            console.log('➡️ total profit', clickedProfit);

            this.router.navigate(['user/home/report', clickedStationId,'month', clickedMonth]);
        }
    }

    loadBarChartYear() {
        this.revenueyearsocket = webSocket<revenuestationyear[]>(environment.wsServerURI + `/ws/station/revenueyear/${this.id}?token=${localStorage.getItem('jwt')}`)
        this.revenueyearsocket.subscribe({
            next: res => {
                this.revstationyear = res
                console.log('Bar Chart year Websocket connected');
                console.log("year data: ", res)
                const dateSet = new Set<string>();
                res.forEach(item => dateSet.add(item.Year));
                const sortedDates = Array.from(dateSet).sort();
                this.Year = sortedDates.map(date => new Date(date).getFullYear().toString());
                this.YearAccount = this.revstationyear.map((item) => item.TotalRevenue);
                this.YearProfit = this.revstationyear.map((item) => item.TotalProfit);
                this.barChartData = {
                    labels: this.Year,
                    datasets: [
                        {
                            label: 'Doanh thu (VNĐ)',
                            data: this.YearAccount,
                            backgroundColor: '#42A5F5'
                        },
                        {
                            label: 'Lợi nhuận (VNĐ)',
                            data: this.YearProfit,
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
    onYearClick(event: any): void {
        const activePoints = event.active;
        if (activePoints && activePoints.length > 0) {
            const chartElement = activePoints[0];
            const chartIndex = chartElement.index;
            const clickedStationId = this.StationId[chartIndex];
            const clickedYear = this.Year[chartIndex];
            console.log('Station Id: ', clickedStationId);
            console.log('Select year:', clickedYear);
            this.router.navigate(['user/home/report', clickedStationId,'year', clickedYear]);
        }
    }

    ngOnDestroy(): void {
        this.revenuesocket?.complete()
        this.revenuesocket?.complete()
    }
}
