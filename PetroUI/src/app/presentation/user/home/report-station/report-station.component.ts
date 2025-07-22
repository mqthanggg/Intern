import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgChartsModule } from 'ng2-charts';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
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
    imports: [NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule]
})

export class ReportStationComponent implements OnInit, OnDestroy {
    @Input() id: number = -1;
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
    revstationDaySocket: { [key: string]: WebSocketSubject<WSrevenuestationday[]> } = {}
    revenuedaysocket: WebSocketSubject<revenuestationday[]> | undefined;
    revstationday: revenuestationday[] = [];
    StationId: number[]=[];
    Day: string[] = [];
    DayAccount: number[] = [];
    DayProfit: number[] = [];
    public barChartData: {
        labels: string[],
        datasets: ChartDataset<'bar'>[]
    } = {
            labels: ['Doanh thu', 'Lợi nhuận'],
            datasets: [
                { data: [0, 0], label: 'VNĐ' }
            ]
        };

    revenuemonthsocket: WebSocketSubject<revenuestationmonth[]> | undefined;
    revstationmonth: revenuestationmonth[] = [];
    Month: string[] = [];
    MonthAccount: number[] = [];
    MonthProfit: number[] = [];
    public barChartMonthData: {
        labels: string[],
        datasets: ChartDataset<'bar'>[]
    } = {
            labels: ['Doanh thu', 'Lợi nhuận'],
            datasets: [
                { data: [0, 0], label: 'VNĐ' }
            ]
        };

    revenueyearsocket: WebSocketSubject<revenuestationyear[]> | undefined;
    revstationyear: revenuestationyear[] = [];
    Year: string[] = [];
    YearAccount: number[] = [];
    YearProfit: number[] = [];
    public barChartYearData: {
        labels: string[],
        datasets: ChartDataset<'bar'>[]
    } = {
            labels: ['Doanh thu', 'Lợi nhuận'],
            datasets: [
                { data: [0, 0], label: 'VNĐ' }
            ]
        };

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

    // Load pie totalliters by name chart
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
    piechartfueldaySocket: { [key: string]: WebSocketSubject<WSrevenuefuelday> } = {}
    piechartfueldaysocket: WebSocketSubject<revenuefuelday[]> | undefined;
    revfuelday: revenuefuelday[] = [];
    DayFuelName: string[] = [];
    Date: string[] = [];
    DayTotalLiters: number[] = [];
    pieChartFuelDateData: any = {};

    piechartfuelmonthSocket: { [key: string]: WebSocketSubject<WSrevenuefuelmonth> } = {}
    piechartfuelmonthsocket: WebSocketSubject<revenuefuelmonth[]> | undefined;
    revfuelmonth: revenuefuelmonth[] = [];
    MonthFuelName: string[] = [];
    Monthpie: string[] = [];
    MonthTotalLiters: number[] = [];
    pieChartFuelMonthData: any = {};

    piechartfuelyearSocket: { [key: string]: WebSocketSubject<WSrevenuefuelyear> } = {}
    piechartfuelyearsocket: WebSocketSubject<revenuefuelyear[]> | undefined;
    revfuelyear: revenuefuelyear[] = [];
    YearFuelName: string[] = [];
    Yearpie: string[] = [];
    YearTotalLiters: number[] = [];
    pieChartFuelYearData: any = {};

    // Load pie totalamount by logtype chart
    public PieChartTypeOption: ChartOptions<'pie'> = {
        responsive: false,
        animation: false,
        plugins: {
            legend: {
                display: true,
                position: 'bottom' as const,
            }
        }
    };
    piecharttypedaySocket: { [key: string]: WebSocketSubject<WSrevenuetypeday[]> } = {}
    piecharttypedaysocket: WebSocketSubject<revenuetypeday[]> | undefined;
    revtypeday: revenuetypeday[] = [];
    DayTypeName: string[] = [];
    DateType: string[] = [];
    DayTotalAmount: number[] = [];
    pieChartTypeDateData: any = {};

    piecharttypemonthSocket: { [key: string]: WebSocketSubject<WSrevenuetypemonth[]> } = {}
    piecharttypemonthsocket: WebSocketSubject<revenuetypemonth[]> | undefined;
    revtypemonth: revenuetypemonth[] = [];
    MonthTypeName: string[] = [];
    Monthtypepie: string[] = [];
    MonthTotalAmount: number[] = [];
    pieChartTypeMonthData: any = {};

    piecharttypeyearSocket: { [key: string]: WebSocketSubject<WSrevenuetypeyear[]> } = {}
    piecharttypeyearsocket: WebSocketSubject<revenuetypeyear[]> | undefined;
    revtypeyear: revenuetypeyear[] = [];
    YearTypeName: string[] = [];
    Yeartypepie: string[] = [];
    YearTotalAmount: number[] = [];
    pieChartTypeYearData: any = {};
    // openDialog() {
    //     this.dialog.open(MyDialogComponent, {
    //         width: '400px',
    //         disableClose: false
    //     });
    // }
    constructor(
        private router: Router,
        private http: HttpClient
    ) { }

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
                            this.revstation[index].TotalRevenue= Datares[index].TotalRevenue
                            this.revstation[index].TotalProfit = Datares[index].TotalProfit
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
        this.loadBarChartDay();
        //===========================================
        this.piechartfueldaysocket = webSocket<revenuefuelday[]>(environment.wsServerURI + `/ws/day/name/${this.id}`);
        this.piechartfueldaysocket.subscribe({
            next: res => {
                this.revfuelday = res
                console.log('Pie Chart date Websocket connected');
                console.log("✔️ date data: ", res)
                this.revstationday.forEach((value, index) => {
                    this.piechartfueldaySocket[value.StationId] = webSocket<WSrevenuefuelday>(environment.wsServerURI + `/ws/day/name/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piechartfueldaySocket[value.StationId].subscribe({
                        next: (Datares: WSrevenuefuelday) => {
                            // res[index].FuelName = Datares.FuelName
                            // res[index].Date = Datares.Date
                            res[index].TotalAmount = Datares.TotalAmount
                            res[index].TotalLiters = Datares.TotalLiters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.StationId}: ${err}`);
                        }
                    })

                })
                this.DayFuelName = this.revfuelday.map((item) => item.FuelName);
                this.Date = this.revfuelday.map((item) => item.Date);
                this.DayTotalLiters = this.revfuelday.map((item) => item.TotalLiters);
                this.pieChartFuelDateData = {
                    labels: this.DayFuelName,
                    datasets: [{
                        data: this.DayTotalLiters,
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

        this.piechartfuelmonthsocket = webSocket<revenuefuelmonth[]>(environment.wsServerURI + `/ws/month/name/${this.id}`);
        this.piechartfuelmonthsocket.subscribe({
            next: res => {
                this.revfuelmonth = res
                console.log('Pie Chart month Websocket connected');
                console.log("✔️ month data: ", res)
                this.revstationmonth.forEach((value, index) => {
                    this.piechartfuelmonthSocket[value.StationId] = webSocket<WSrevenuefuelmonth>(environment.wsServerURI + `/ws/month/name/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piechartfuelmonthSocket[value.StationId].subscribe({
                        next: (Datares: WSrevenuefuelmonth) => {
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

        this.piechartfuelyearsocket = webSocket<revenuefuelyear[]>(environment.wsServerURI + `/ws/year/name/${this.id}`);
        this.piechartfuelyearsocket.subscribe({
            next: res => {
                this.revfuelyear = res
                console.log('Pie Chart year Websocket connected');
                console.log("✔️ year data: ", res)
                this.revstationyear.forEach((value, index) => {
                    this.piechartfuelyearSocket[value.StationId] = webSocket<WSrevenuefuelyear>(environment.wsServerURI + `/ws/year/name/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piechartfuelyearSocket[value.StationId].subscribe({
                        next: (Datares: WSrevenuefuelyear) => {
                            res[index].FuelName = Datares.FuelName
                            res[index].Year = Datares.Year
                            res[index].TotalAmount = Datares.TotalAmount
                            res[index].TotalLiters = Datares.TotalLiters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.StationId}: ${err}`);
                        }
                    })

                })
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
        //===========================================
        this.piecharttypedaysocket = webSocket<revenuetypeday[]>(environment.wsServerURI + `/ws/day/type/${this.id}`);
        this.piecharttypedaysocket.subscribe({
            next: res => {
                this.revtypeday = res
                console.log('Pie Chart date type Websocket connected');
                console.log("date type data: ", res)
                this.revtypeday.forEach((value, index) => {
                    this.piecharttypedaySocket[value.StationId] = webSocket<WSrevenuetypeday[]>(environment.wsServerURI + `/ws/day/type/${this.id}?token=${localStorage.getItem('jwt')}`)
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

        this.piecharttypemonthsocket = webSocket<revenuetypemonth[]>(environment.wsServerURI + `/ws/month/type/${this.id}`);
        this.piecharttypemonthsocket.subscribe({
            next: res => {
                this.revtypemonth = res
                console.log('✔️ Pie Chart month type Websocket connected');
                console.log("month type data: ", res)
                this.revtypemonth.forEach((value, index) => {
                    this.piecharttypemonthSocket[value.StationId] = webSocket<WSrevenuetypemonth[]>(environment.wsServerURI + `/ws/month/type/${this.id}?token=${localStorage.getItem('jwt')}`)
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
                this.Month = this.revtypemonth.map((item) => item.Month);
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

        this.piecharttypeyearsocket = webSocket<revenuetypeyear[]>(environment.wsServerURI + `/ws/year/type/${this.id}`);
        this.piecharttypeyearsocket.subscribe({
            next: res => {
                this.revtypeyear = res
                console.log('Pie Chart year type Websocket connected');
                console.log("year type data: ", res)
                this.revtypeyear.forEach((value, index) => {
                    this.piecharttypeyearSocket[value.StationId] = webSocket<WSrevenuetypeyear[]>(environment.wsServerURI + `/ws/year/type/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piecharttypeyearSocket[value.StationId].subscribe({
                        next: Datares => {
                            res[index].LogTypeName = Datares[index].LogTypeName
                            res[index].TotalAmount = Datares[index].TotalAmount
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.StationId}: ${err}`);
                        }
                    })
                })
                this.YearTypeName = this.revtypeyear.map((item) => item.LogTypeName);
                this.YearTotalAmount = this.revtypeyear.map((item) => item.TotalAmount);
                this.Year = this.revtypeyear.map((item) => item.Year);
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
                this.StationId = this.revstationday.map((item) =>item.StationId);
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
    onChartClick(event: any): void {
        const activePoints = event.active;
        if (activePoints && activePoints.length > 0) {
            const chartElement = activePoints[0];
            const chartIndex = chartElement.index;
            const clickedStationId = this.StationId[chartIndex];
            const clickedDate = this.Day[chartIndex];
            const clickedRevenue = this.DayAccount[chartIndex];
            const clickedProfit = this.DayProfit[chartIndex];
            console.log('Station Id: ', clickedStationId);
            console.log('🟡 Ngày được chọn:', clickedDate);
            console.log('➡️ Doanh thu:', clickedRevenue);
            console.log('➡️ Lợi nhuận:', clickedProfit);
            // Ví dụ: Hiển thị popup hoặc điều hướng
            // this.router.navigate(['/doanh-thu/ngay', clickedDate]);
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
                this.barChartMonthData = {
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
                this.barChartYearData = {
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
    ngOnDestroy(): void {
        this.revenuesocket?.complete()
        for (const socket in this.revstationSocket, this.revstationDaySocket) {
            this.revstationSocket[socket].complete()
            this.revstationDaySocket[socket].complete()
        }
    }
}
