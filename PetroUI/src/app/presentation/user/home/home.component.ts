import { Component, OnInit, ViewChild } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { ChartOptions, ChartDataset, ChartEvent } from 'chart.js';
import { Router } from '@angular/router';
import { WebSocketService } from './../../../services/web-socket.service';
import { totalFuelName, totalLogType, totalrevenue, totalrevenue7day, totalStationName, WStotalFuelName, WStotalLogType, WStotalrevenue, WStotalrevenue7day, WStotalStationName } from './home-station-record';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  imports: [
    CommonModule,
    CurrencyPipe,
    NgChartsModule
  ]
})

export class HomeComponent implements OnInit {
  constructor(
    private router: Router,
    private wsService: WebSocketService
  ) { }

  stationData: any = {};
  barData: any = {};
  lineChartData: any = {};


  baryData: any = {};
  stationIdList: number[] = []

  stationsocket: WebSocket | undefined;
  barsocket: WebSocket | undefined;
  barSocket: WebSocketSubject<WStotalStationName[]> | undefined;
  listStation: totalStationName[] = [];
  station: string[] = [];
  profit: number[] = [];
  revenue: number[] = [];

  revenuesocket: WebSocketSubject<totalrevenue[]> | undefined;
  revenueSocket: WebSocketSubject<WStotalrevenue> | undefined;
  revenueData?: totalrevenue;

  piecharsocket: WebSocketSubject<totalFuelName[]> | undefined;
  piecharSocket: WebSocketSubject<WStotalFuelName[]> | undefined;
  totalNameData: totalFuelName[] = [];
  fuelname: string[] = [];
  totalliters: number[] = [];
  totalamount: number[] = [];
  pieChartAccountData: any = {};
  pieChartLittersData: any = {};

  linecharsocket: WebSocketSubject<totalrevenue7day[]> | undefined;
  linecharSocket: WebSocketSubject<WStotalrevenue7day[]> | undefined;
  totalDate: totalrevenue7day[] = [];
  DataLable: string[] = [];

  barycharsocket: WebSocketSubject<totalLogType[]> | undefined;
  barycharSocket: WebSocketSubject<WStotalLogType[]> | undefined;
  totalLog: totalLogType[] = [];
  chartBaryLogName: string[] = [];
  chartBaryDataAccounts: number[] = [];

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
  } =
    {
      labels: ['Doanh thu', 'Lợi nhuận'],
      datasets: [
        { data: [0, 0], label: 'VNĐ' }
      ]
    };

  public LineChartOption: ChartOptions<'line'> = {
    responsive: false,
    plugins: {
      legend: {
        display: true,
        labels: {
          usePointStyle: true,
          pointStyle: 'line'
        }
      }
    },
    elements: {
      point: { radius: 0 }
    },
    scales: {
      y: { beginAtZero: true }
    }
  };

  public PieBarChartOption: ChartOptions<'pie'> = {
    responsive: false,
    animation: false,
    plugins: {
      legend: {
        display: true,
        position: 'bottom' as const,
      }
    }
  };

  public baryChartOptions: ChartOptions<'bar'> = {
    responsive: false,
    indexAxis: 'y',
    plugins: {
      legend: { display: true },
    },
    scales: {
      x: { beginAtZero: true },
    },
  }

  onChartClick(event: { event?: ChartEvent, active?: any[] }) {
    if (event.active && event.active.length > 0) {
      const chartElement = event.active[0];
      const dataIndex = chartElement.index;
      const stationId = this.stationIdList[dataIndex];
      if (!stationId) {
        console.error('🚨 stationId is undefined!');
        return;
      }

      this.router.navigate(['user/home/report', stationId]);
    }
  }

  ngOnInit(): void {
    this.connectWebsocket();
  }
  handleBarChartData(event: MessageEvent): void {
    const rawData = JSON.parse(event.data);
    console.log('Received revenue and profit:', rawData);
    const filteredData = rawData.filter((item: any) =>
      item.TotalRevenue > 0 || item.TotalProfit > 0
    );
    this.barData = filteredData;
    const stations = filteredData.map((item: any) => item.StationName);
    const revenue = filteredData.map((item: any) => item.TotalRevenue);
    const profit = filteredData.map((item: any) => item.TotalProfit);
    this.stationIdList = filteredData.map((item: any) => item.StationId);
    this.barChartData = {
      labels: stations,
      datasets: [
        {
          label: 'Doanh thu (VNĐ)',
          data: revenue,
          backgroundColor: '#42A5F5'
        },
        {
          label: 'Lợi nhuận (VNĐ)',
          data: profit,
          backgroundColor: '#66BB6A'
        }
      ]
    };
  }

  // handleBarChartData(event: any) {
  //   try {
  //     const rawData = typeof event.data === 'string' ? JSON.parse(event.data) : event;

  //     if (!Array.isArray(rawData)) {
  //       throw new Error('Dữ liệu không phải là mảng');
  //     }
  //     const data: totalStationName[] = rawData;
  //     const validData = data.filter(
  //       item =>
  //         item &&
  //         typeof item.TotalRevenue === 'number' &&
  //         typeof item.TotalProfit === 'number' &&
  //         item.TotalRevenue > 0 || item.TotalProfit > 0
  //     );
  //     this.barData = validData;
  //     this.station = validData.map(item => item.StationName);
  //     this.revenue = validData.map(item => item.TotalRevenue);
  //     this.profit = validData.map(item => item.TotalProfit);
  //     this.barChartData = {
  //       labels: this.station,
  //       datasets: [
  //         {
  //           label: 'Doanh thu (VNĐ)',
  //           data: this.revenue,
  //           backgroundColor: '#42A5F5'
  //         },
  //         {
  //           label: 'Lợi nhuận (VNĐ)',
  //           data: this.profit,
  //           backgroundColor: '#66BB6A'
  //         }
  //       ]
  //     };
  //     console.log('Bar chart updated:', this.barChartData);
  //   } catch (error) {
  //     console.error('Lỗi khi xử lý dữ liệu WebSocket:', error);
  //   }
  // }

  connectWebsocket(): void {
    this.wsService.connect('barchar', environment.wsServerURI + '/ws/sumrevenue');
    this.barsocket = this.wsService.getSocket('barchar');
    if (this.barsocket) {
      this.barsocket.onopen = () => console.log('bar char websocket connected');
      this.barsocket.onmessage = (event) => this.handleBarChartData(event);
    }
    // this.barsocket = webSocket<totalStationName[]>(environment.wsServerURI + `/ws/sumrevenue`);
    // this.barsocket.subscribe({
    //   next: res => {
    //     this.listStation = res
    //     console.log('bar Chart date Websocket connected');
    //     console.log("✔️ bar data: ", res)
    //     this.listStation.forEach((value, index) => {
    //       this.barSocket = webSocket<WStotalStationName[]>(environment.wsServerURI + `/ws/sumrevenue?token=${localStorage.getItem('jwt')}`)
    //       this.barSocket.subscribe({
    //         next: (Datares: WStotalStationName[]) => {
    //           res[index].StationId = Datares[index].StationId
    //           res[index].StationName = Datares[index].StationName
    //           res[index].TotalRevenue = Datares[index].TotalRevenue
    //           res[index].TotalProfit = Datares[index].TotalProfit
    //         },
    //       })
    //       this.station = this.listStation.map(item => item.StationName);
    //       this.revenue = this.listStation.map(item => item.to);
    //       this.profit =  this.listStation.map(item => item.TotalRevenue)

    //       this.barChartData = {
    //         labels: stations,
    //         datasets: [
    //           {
    //             label: 'Doanh thu (VNĐ)',
    //             data: revenue,
    //             backgroundColor: '#42A5F5'
    //           },
    //           {
    //             label: 'Lợi nhuận (VNĐ)',
    //             data: profit,
    //             backgroundColor: '#66BB6A'
    //           }
    //         ]
    //       };
    //     })
    //   },
    //   error: err => {
    //     console.error(err);
    //   }
    // })
    //===============================================================
    // ✅ Load sum revenue 
    this.revenueSocket = webSocket<WStotalrevenue>(environment.wsServerURI + '/ws/revenue');
    this.revenueSocket.subscribe({
      next: (data: WStotalrevenue) => {
        this.revenueData = {
          TotalLiter: data.TotalLiters,
          TotalRevenue: data.TotalRevenue,
          TotalProfit: data.TotalProfit
        };
        console.log('✔️ Received 1 record:', this.revenueData);
      },
      error: (err) => {
        console.error('WebSocket error:', err);
      }
    })

    // ✅ Load sum station
    this.stationsocket = new WebSocket(environment.wsServerURI + '/ws/station');
    this.stationsocket.onopen = () => console.log('Station Websocket connected');
    this.stationsocket.onmessage = (event) => {
      const SumStation = JSON.parse(event.data);
      console.log('✔️ Total stations:', SumStation);
      this.stationData = SumStation;
    };
    this.stationsocket.onerror = (error) => console.error('Station Websocket error:', error);
    this.stationsocket.onclose = () => console.warn('Station Websocket closed');

    // ✅ Load 2 chart pie sum revenue name
    this.piecharsocket = webSocket<totalFuelName[]>(environment.wsServerURI + `/ws/sumrenuename`);
    this.piecharsocket.subscribe({
      next: res => {
        this.totalNameData = res
        console.log('Pie Chart date Websocket connected');
        console.log("✔️ date data: ", res)
        this.totalNameData.forEach((value, index) => {
          this.piecharSocket = webSocket<WStotalFuelName[]>(environment.wsServerURI + `/ws/sumrenuename?token=${localStorage.getItem('jwt')}`)
          this.piecharSocket.subscribe({
            next: (Datares: WStotalFuelName[]) => {
              res[index].FuelName = Datares[index].FuelName
              res[index].TotalAmount = Datares[index].TotalAmount
              res[index].TotalLiters = Datares[index].TotalLiters
            },
            error: (err) => {
              console.error(`Error at station: ${err}`);
            }
          })
        })
        this.fuelname = this.totalNameData.map((item) => item.FuelName);
        this.totalamount = this.totalNameData.map((item) => item.TotalAmount);
        this.totalliters = this.totalNameData.map((item) => item.TotalLiters)
        this.pieChartAccountData = {
          labels: this.fuelname,
          datasets: [{
            data: this.totalamount,
            backgroundColor: [
              '#FF6384',
              '#36A2EB',
              '#FFCE56',
              '#4BC0C0',
              '#9966FF'
            ]
          }]
        };
        this.pieChartLittersData = {
          labels: this.fuelname,
          datasets: [{
            data: this.totalliters,
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

    // ✅ Load chart line sum revenue name
    const defaultColors = ['#42A5F5', '#66BB6A', '#FFA726', '#EF5350', '#AB47BC', '#FFEB3B', '#26C6DA', '#8D6E63'];
    this.linecharsocket = webSocket<totalrevenue7day[]>(environment.wsServerURI + `/ws/sumrevenueday`);
    this.linecharsocket.subscribe({
      next: res => {
        this.totalDate = res
        console.log('Line Chart date Websocket connected');
        console.log("✔️ line data: ", res)
        this.totalNameData.forEach((value, index) => {
          this.linecharSocket = webSocket<WStotalrevenue7day[]>(environment.wsServerURI + `/ws/sumrevenueday?token=${localStorage.getItem('jwt')}`)
          this.linecharSocket.subscribe({
            next: (Datares: WStotalrevenue7day[]) => {
              res[index].Date = Datares[index].Date
              res[index].TotalAmount = Datares[index].TotalAmount
              res[index].StationName = Datares[index].StationName
            },
            error: (err) => {
              console.error(`Error at station: ${err}`);
            }
          })
        })
        const dateSet = new Set<string>();
        this.totalDate.forEach(item => dateSet.add(item.Date));
        const sortedDates = Array.from(dateSet).sort();
        this.DataLable = sortedDates.map(date =>
          new Date(date).toLocaleDateString('vi-VN')
        );
        const stationMap = new Map<string, { [date: string]: number }>();
        this.totalDate.forEach(item => {
          if (!stationMap.has(item.StationName)) {
            stationMap.set(item.StationName, {});
          }
          stationMap.get(item.StationName)![item.Date] = item.TotalAmount;
        });
        const datasets = Array.from(stationMap.entries()).map(([station, values], index) => {
          const data = sortedDates.map(date => {
            const value = values[date];
            return value === 0 || value === undefined ? null : value;
          });
          return {
            label: station,
            data,
            tension: 0.4,
            borderWidth: 3,
            borderColor: defaultColors[index % defaultColors.length],
            pointRadius: 5,
            pointHoverRadius: 7,
            pointBackgroundColor: defaultColors[index % defaultColors.length],
            pointBorderColor: defaultColors[index % defaultColors.length],
            spanGaps: true
          };
        });
        this.lineChartData = {
          labels: this.DataLable,
          datasets
        };
      },

      error: err => {
        console.error(err);
      }
    })

    // ✅ Load chart bar y sum revenue by type
    this.barycharsocket = webSocket<totalLogType[]>(environment.wsServerURI + `/ws/sumrenuetype`);
    this.barycharsocket.subscribe({
      next: res => {
        this.totalLog = res
        console.log('bar y Chart date Websocket connected');
        console.log("✔️ bary data: ", res)
        this.totalLog.forEach((value, index) => {
          this.barycharSocket = webSocket<WStotalLogType[]>(environment.wsServerURI + `/ws/sumrenuetype?token=${localStorage.getItem('jwt')}`)
          this.barycharSocket.subscribe({
            next: (Datares: WStotalLogType[]) => {
              res[index].LogTypeName = Datares[index].LogTypeName
              res[index].TotalAmount = Datares[index].TotalAmount
            },
          })
          this.chartBaryLogName = this.totalLog.map(item => item.LogTypeName);
          this.chartBaryDataAccounts = this.totalLog.map(item => item.TotalAmount);
          this.baryData = {
            labels: this.chartBaryLogName,
            datasets: [
              {
                label: 'Doanh thu (VNĐ)',
                data: this.chartBaryDataAccounts,
                backgroundColor: '#42A5F5'
              },
            ]
          };

        })
      },
      error: err => {
        console.error(err);
      }
    })
  }

  ngOnDestroy(): void {
    if (this.revenuesocket && this.stationsocket && this.barsocket) {
      this.stationsocket.close();
      this.barsocket?.close();
    }
  }
}
