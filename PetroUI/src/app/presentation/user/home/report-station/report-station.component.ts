import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { NgChartsModule } from 'ng2-charts';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { CommonModule } from '@angular/common';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket'
import {
    revenuefuelyear, revenuefuelday, revenuefuelmonth, revenuestation, revenuestationday, revenuestationmonth,
    revenuestationyear, revenuetypeday, revenuetypemonth, revenuetypeyear, WSrevenuefuelmonth, WSrevenuefuelday,
    WSrevenuefuelyear, WSrevenuestation, WSrevenuestationday, WSrevenuestationmonth, WSrevenuestationyear,
    WSrevenuetypeday, WSrevenuetypemonth, WSrevenuetypeyear
} from './model/sumrevenuestation-record';
import { ChartDataset, ChartEvent, ChartOptions } from 'chart.js';
import { FormsModule } from '@angular/forms';
import { WebSocketService } from '../../../../services/web-socket.service';
@Component({
    standalone: true,
    selector: 'app-report-station',
    templateUrl: './report-station.component.html',
    imports: [NgChartsModule, ReactiveFormsModule, CommonModule, FormsModule]
})

export class ReportStationComponent implements OnInit, OnDestroy {
    @Input() id: number = -1;
    revstationSocket: { [key: string]: WebSocketSubject<WSrevenuestation> } = {}
    revenuesocket: WebSocketSubject<revenuestation[]> | undefined;
    revstation: revenuestation[] = [];
    DataAccount: number[] = [];
    DataLitters: number[] = [];
    DataProfit: number[] = [];
    TotalLitters = 0;
    TotalRevenue = 0;
    TotalProfit = 0;
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
    revstationDaySocket: { [key: string]: WebSocketSubject<WSrevenuestationday> } = {}
    revenuedaysocket: WebSocketSubject<any> | undefined;
    revstationday: revenuestationday[] = [];
    Date: string[] = [];
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

    revstationMonthSocket: { [key: string]: WebSocketSubject<WSrevenuestationmonth> } = {}
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

    revstationYearSocket: { [key: string]: WebSocketSubject<WSrevenuestationyear> } = {}
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

    // Load pie totalLiters by name chart
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
    Day: string[] = [];
    Daytotalliters: number[] = [];
    pieChartFuelDateData: any = {};

    piechartfuelmonthSocket: { [key: string]: WebSocketSubject<WSrevenuefuelmonth> } = {}
    piechartfuelmonthsocket: WebSocketSubject<revenuefuelmonth[]> | undefined;
    revfuelmonth: revenuefuelmonth[] = [];
    MonthFuelName: string[] = [];
    Monthpie: string[] = [];
    Monthtotalliters: number[] = [];
    pieChartFuelMonthData: any = {};

    piechartfuelyearSocket: { [key: string]: WebSocketSubject<WSrevenuefuelyear> } = {}
    piechartfuelyearsocket: WebSocketSubject<revenuefuelyear[]> | undefined;
    revfuelyear: revenuefuelyear[] = [];
    YearFuelName: string[] = [];
    Yearpie: string[] = [];
    Yeartotalliters: number[] = [];
    pieChartFuelYearData: any = {};

    // Load pie totalAmount by logtype chart
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
    piecharttypedaySocket: { [key: string]: WebSocketSubject<WSrevenuetypeday> } = {}
    piecharttypedaysocket: WebSocketSubject<revenuetypeday[]> | undefined;
    revtypeday: revenuetypeday[] = [];
    DayTypeName: string[] = [];
    DateType: string[] = [];
    DayTotalAmount: number[] = [];
    pieChartTypeDateData: any = {};

    piecharttypemonthSocket: { [key: string]: WebSocketSubject<WSrevenuetypemonth> } = {}
    piecharttypemonthsocket: WebSocketSubject<revenuetypemonth[]> | undefined;
    revtypemonth: revenuetypemonth[] = [];
    MonthTypeName: string[] = [];
    Monthtypepie: string[] = [];
    MonthTotalAmount: number[] = [];
    pieChartTypeMonthData: any = {};

    piecharttypeyearSocket: { [key: string]: WebSocketSubject<WSrevenuetypeyear> } = {}
    piecharttypeyearsocket: WebSocketSubject<revenuetypeyear[]> | undefined;
    revtypeyear: revenuetypeyear[] = [];
    YearTypeName: string[] = [];
    Yeartypepie: string[] = [];
    YearTotalAmount: number[] = [];
    pieChartTypeYearData: any = {};
    //==========================================
    piesocket: WebSocket | undefined;
    stationidList: number[] = []
    barData: any = {};
    date: string[] = [];
    onChartClick(event: { event?: ChartEvent, active?: any[] }) {
        if (event.active && event.active.length > 0) {
            const chartElement = event.active[0];
            const dataIndex = chartElement.index;

            const stationid = this.stationidList[dataIndex];
            const datatime = this.date[dataIndex]
            console.log("get date: ", datatime)
            if (!stationid) {
                console.error('🚨 stationid is undefined!');
                return;
            }
            this.router.navigate(['user/home/report', stationid]);
        }
    }

    handleBarChartData(event: MessageEvent): void {
        const rawData = JSON.parse(event.data);
        console.log('-- Received revenue and profit:', rawData);

        const filteredData = rawData.filter((item: any) =>
            item.totalRevenue > 0 || item.totalProfit > 0
        );
        this.loadBarChartDay
    }

    constructor(
        private router: Router,
        private http: HttpClient,
        private wsService: WebSocketService
    ) { }

    ngOnInit(): void {
        this.wsService.connect('piefuelchar', environment.wsServerURI + `/ws/day/name/${this.id}`);
        this.piesocket = this.wsService.getSocket('piefuelchar');
        if (this.piesocket) {
            this.piesocket.onopen = () => console.log('bar char websocket connected');
            this.piesocket.onmessage = (event) => this.handleBarChartData(event);
        }

        this.revenuesocket = webSocket<revenuestation[]>(environment.wsServerURI + `/ws/station/${this.id}`)
        this.revenuesocket.subscribe({
            next: res => {
                this.revstation = res;
                this.revstation.forEach((value, index) => {
                    this.revstationSocket[value.stationId] = webSocket<WSrevenuestation>(environment.wsServerURI + `/ws/station/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.revstationSocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuestation) => {
                            this.revstation[index].totalLiters = Datares.liter
                            this.revstation[index].totalRevenue = Datares.revenue
                            this.revstation[index].totalProfit = Datares.profit
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })
                })
                if (this.revstation == null) {
                    console.log("??? data bar enity");
                }
                this.DataAccount = this.revstation.map((item) => item.totalRevenue);
                this.TotalRevenue = this.DataAccount.reduce((acc, val) => acc + val, 0);
                this.DataLitters = this.revstation.map((item) => item.totalLiters);
                this.TotalLitters = this.DataLitters.reduce((acc, val) => acc + val, 0);
                this.DataProfit = this.revstation.map((item) => item.totalProfit);
                this.TotalProfit = this.DataProfit.reduce((acc, val) => acc + val, 0);
                if (!this.revstation || this.revstation.length === 0) {
                    console.warn("No data in revstation !!!");
                    return;
                }
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
                console.log("date data: ", res)
                this.revstationday.forEach((value, index) => {
                    this.piechartfueldaySocket[value.stationId] = webSocket<WSrevenuefuelday>(environment.wsServerURI + `/ws/day/name/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piechartfueldaySocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuefuelday) => {
                            res[index].fuelName = Datares.fuelName
                            res[index].date = Datares.date
                            res[index].totalAmount = Datares.amount
                            res[index].totalLiters = Datares.liters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })

                })
                this.DayFuelName = this.revfuelday.map((item) => item.fuelName);
                this.Date = this.revfuelday.map((item) => item.date);
                this.Daytotalliters = this.revfuelday.map((item) => item.totalLiters);
                this.pieChartFuelDateData = {
                    labels: this.DayFuelName,
                    datasets: [{
                        data: this.Daytotalliters,
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
                console.log("month data: ", res)
                this.revstationmonth.forEach((value, index) => {
                    this.piechartfuelmonthSocket[value.stationId] = webSocket<WSrevenuefuelmonth>(environment.wsServerURI + `/ws/month/name/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piechartfuelmonthSocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuefuelmonth) => {
                            res[index].fuelName = Datares.fuelName
                            res[index].month = Datares.month
                            res[index].totalAmount = Datares.amount
                            res[index].totalLiters = Datares.liters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })

                })
                this.MonthFuelName = this.revfuelmonth.map((item) => item.fuelName);
                this.Monthpie = this.revfuelmonth.map((item) => item.month);
                this.Monthtotalliters = this.revfuelmonth.map((item) => item.totalLiters);
                this.pieChartFuelMonthData = {
                    labels: this.MonthFuelName,
                    datasets: [{
                        data: this.Monthtotalliters,
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
                console.log("year data: ", res)
                this.revstationyear.forEach((value, index) => {
                    this.piechartfuelyearSocket[value.stationId] = webSocket<WSrevenuefuelyear>(environment.wsServerURI + `/ws/year/name/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piechartfuelyearSocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuefuelyear) => {
                            res[index].fuelName = Datares.fuelName
                            res[index].year = Datares.year
                            res[index].totalAmount = Datares.amount
                            res[index].totalLiters = Datares.liters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })

                })
                this.YearFuelName = this.revfuelyear.map((item) => item.fuelName);
                this.Yearpie = this.revfuelyear.map((item) => item.year);
                this.Yeartotalliters = this.revfuelyear.map((item) => item.totalLiters);
                this.pieChartFuelYearData = {
                    labels: this.YearFuelName,
                    datasets: [{
                        data: this.Yeartotalliters,
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
                    this.piecharttypedaySocket[value.stationId] = webSocket<WSrevenuetypeday>(environment.wsServerURI + `/ws/day/type/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piecharttypedaySocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuetypeday) => {
                            res[index].logTypeName = Datares.logName
                            res[index].date = Datares.date
                            res[index].totalAmount = Datares.amount
                            // res[index].totalLiters = Datares.liters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })

                })
                this.DayTypeName = this.revtypeday.map((item) => item.logTypeName);
                this.Date = this.revtypeday.map((item) => item.date);
                this.DayTotalAmount = this.revtypeday.map((item) => item.totalAmount);
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
                console.log('Pie Chart month type Websocket connected');
                console.log("month type data: ", res)
                this.revtypemonth.forEach((value, index) => {
                    this.piecharttypemonthSocket[value.stationId] = webSocket<WSrevenuetypemonth>(environment.wsServerURI + `/ws/month/type/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piecharttypemonthSocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuetypemonth) => {
                            res[index].logTypeName = Datares.logName
                            res[index].month = Datares.month
                            res[index].totalAmount = Datares.amount
                            // res[index].totalLiters = Datares.liters
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })
                })
                this.MonthTypeName = this.revtypemonth.map((item) => item.logTypeName);
                this.Month = this.revtypemonth.map((item) => item.month);
                this.MonthTotalAmount = this.revtypemonth.map((item) => item.totalAmount);
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
                    this.piecharttypeyearSocket[value.stationId] = webSocket<WSrevenuetypeyear>(environment.wsServerURI + `/ws/year/type/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.piecharttypeyearSocket[value.stationId].subscribe({
                        next: Datares => {
                            res[index].logTypeName = Datares.logName
                            res[index].year = Datares.year
                            res[index].totalAmount = Datares.amount
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })
                })
                this.YearTypeName = this.revtypeyear.map((item) => item.logTypeName);
                this.YearTotalAmount = this.revtypeyear.map((item) => item.totalAmount);
                this.Year = this.revtypeyear.map((item) => item.year);
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
        this.revenuedaysocket = webSocket<any>(environment.wsServerURI + `/ws/station/revenueday/${this.id}`)
        this.revenuedaysocket.subscribe({
            next: res => {
                this.revstationday = res
                console.log('-- Bar Chart date Websocket connected');
                console.log("date data: ", res)
                this.revstationday.forEach((value, index) => {
                    this.revstationDaySocket[value.stationId] = webSocket<WSrevenuestationday>(environment.wsServerURI + `/ws/station/revenueday/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.revstationDaySocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuestationday) => {
                            res[index].stationId = Datares.id
                            res[index].stationName = Datares.name
                            res[index].date = Datares.date
                            res[index].totalRevenue = Datares.revenue
                            res[index].totalProfit = Datares.profit
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })

                })
                // const dateSet = new Set<string>();
                // res.forEach(item => dateSet.add(item.date));
                // const sortedDates = Array.from(dateSet).sort();
                // this.Day = sortedDates.map(date => new Date(date).toLocaleDateString('vi-VN'));
                this.Date = this.revstationday.map((item) => item.date);
                console.log(this.Date);

                this.DayAccount = this.revstationday.map((item) => item.totalRevenue);
                this.DayProfit = this.revstationday.map((item) => item.totalProfit);
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
    loadBarChartMonth() {
        this.revenuemonthsocket = webSocket<revenuestationmonth[]>(environment.wsServerURI + `/ws/station/revenuemonth/${this.id}`)
        this.revenuemonthsocket.subscribe({
            next: res => {
                this.revstationmonth = res
                console.log('-- Bar Chart month Websocket connected');
                console.log("month data: ", res)
                this.revstationday.forEach((value, index) => {
                    this.revstationMonthSocket[value.stationId] = webSocket<WSrevenuestationmonth>(environment.wsServerURI + `/ws/station/revenuemonth/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.revstationMonthSocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuestationmonth) => {
                            res[index].month = Datares.month
                            res[index].totalRevenue = Datares.revenue
                            res[index].totalProfit = Datares.profit
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })

                })
                this.Month = this.revstationmonth.map((item) => item.month);
                this.MonthAccount = this.revstationmonth.map((item) => item.totalRevenue);
                this.MonthProfit = this.revstationmonth.map((item) => item.totalProfit);
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
        this.revenueyearsocket = webSocket<revenuestationyear[]>(environment.wsServerURI + `/ws/station/revenueyear/${this.id}`)
        this.revenueyearsocket.subscribe({
            next: res => {
                this.revstationyear = res
                console.log('Bar Chart year Websocket connected');
                console.log("year data: ", res)
                this.revstationday.forEach((value, index) => {
                    this.revstationYearSocket[value.stationId] = webSocket<WSrevenuestationyear>(environment.wsServerURI + `/ws/station/revenueyear/${this.id}?token=${localStorage.getItem('jwt')}`)
                    this.revstationYearSocket[value.stationId].subscribe({
                        next: (Datares: WSrevenuestationyear) => {
                            res[index].year = Datares.year
                            res[index].totalRevenue = Datares.revenue
                            res[index].totalProfit = Datares.profit
                        },
                        error: (err) => {
                            console.error(`Error at station ${value.stationId}: ${err}`);
                        }
                    })

                })
                const dateSet = new Set<string>();
                res.forEach(item => dateSet.add(item.year));
                const sortedDates = Array.from(dateSet).sort();
                this.Year = sortedDates.map(date => new Date(date).getFullYear().toString());
                this.YearAccount = this.revstationyear.map((item) => item.totalRevenue);
                this.YearProfit = this.revstationyear.map((item) => item.totalProfit);
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
