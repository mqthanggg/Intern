import { Component, OnInit } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { ChartOptions, ChartDataset, ChartEvent } from 'chart.js';
import { Router } from '@angular/router';
import { totalFuelName, totalLogType, totalrevenue, totalrevenue7day, totalStationName } from './home-station-record';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { TitleService } from '../../../infrastructure/services/title.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  imports: [CommonModule, CurrencyPipe, NgChartsModule]
})

export class HomeComponent implements OnInit {
  constructor(
    private router: Router, private titleService: TitleService
  ) { }

  stationData: any = {};
  barData: any = {};
  lineChartData: any = {};

  baryData: any = {};
  stationIdList: number[] = []

  stationsocket: WebSocket | undefined;
  barsocket: WebSocketSubject<totalStationName[]> | undefined;
  listStation: totalStationName[] = [];
  stationId: number[] = [];
  station: string[] = [];
  profit: number[] = [];
  revenue: number[] = [];

  revenuesocket: WebSocketSubject<totalrevenue> | undefined
  revenueData?: totalrevenue;

  piecharsocket: WebSocketSubject<totalFuelName[]> | undefined;
  totalNameData: totalFuelName[] = [];
  fuelname: string[] = [];
  totalliters: number[] = [];
  totalamount: number[] = [];
  pieChartAccountData: any = {};
  pieChartLittersData: any = {};

  linecharsocket: WebSocketSubject<totalrevenue7day[]> | undefined;
  totalDate: totalrevenue7day[] = [];
  DataLable: string[] = [];

  barycharsocket: WebSocketSubject<totalLogType[]> | undefined;
  totalLog: totalLogType[] = [];
  chartBaryLogName: string[] = [];
  chartBaryDataAccounts: number[] = [];
  showLogs = true;

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
      const stationId = this.stationId[dataIndex];
      if (!stationId) {
        console.error('🚨 stationId is undefined!');
        return;
      }
      this.router.navigate(['user/home/report', stationId]);
    }
  }

  ngOnInit(): void {
    this.showLogs = true;
    setTimeout(() => {
      this.titleService.updateTitle("Home")
    }, 0)
    this.barsocket = webSocket<totalStationName[]>(environment.wsServerURI + `/ws/sumrevenue?token=${localStorage.getItem('jwt')}`);
    this.barsocket.subscribe({
      next: (res) => {
        // console.log('bar Chart date Websocket connected');
        // console.log("✔️ bar data: ", res);
        const filtered = res.filter(s => s.TotalRevenue > 0 || s.TotalProfit > 0);
        this.stationId = filtered.map(item => item.StationId);
        this.station = filtered.map(item => item.StationName);
        this.revenue = filtered.map(item => item.TotalRevenue);
        this.profit = filtered.map(item => item.TotalProfit);

        this.barChartData = {
          labels: this.station,
          datasets: [
            {
              label: 'Doanh thu (VNĐ)',
              data: this.revenue,
              backgroundColor: '#42A5F5'
            },
            {
              label: 'Lợi nhuận (VNĐ)',
              data: this.profit,
              backgroundColor: '#66BB6A'
            }
          ]
        };
      }
    });

    // ✅ Load sum revenue 
    this.revenuesocket = webSocket<totalrevenue>(environment.wsServerURI + `/ws/revenue?token=${localStorage.getItem('jwt')}`);
    this.revenuesocket.subscribe({
      next: (data: totalrevenue) => {
        this.revenueData = {
          TotalLiters: data.TotalLiters,
          TotalRevenue: data.TotalRevenue,
          TotalProfit: data.TotalProfit
        };
        // console.log('✔️ Received data:', this.revenueData);
      },
      error: (err) => {
        console.error('WebSocket error:', err);
      }
    })

    // ✅ Load sum station
    this.stationsocket = new WebSocket(environment.wsServerURI + `/ws/station?token=${localStorage.getItem('jwt')}`);
    this.stationsocket.onopen = () => console.log('Station Websocket connected');
    this.stationsocket.onmessage = (event) => {
      const SumStation = JSON.parse(event.data);
      // console.log('✔️ Total stations:', SumStation);
      this.stationData = SumStation;
    };
    this.stationsocket.onerror = (error) => console.error('Station Websocket error:', error);
    this.stationsocket.onclose = () => console.warn('Station Websocket closed');

    // ✅ Load 2 chart pie sum revenue name
    this.piecharsocket = webSocket<totalFuelName[]>(environment.wsServerURI + `/ws/sumrenuename?token=${localStorage.getItem('jwt')}`);
    this.piecharsocket.subscribe({
      next: res => {
        this.totalNameData = res
        // console.log('Pie Chart date Websocket connected');
        // console.log("✔️ date data: ", res);
        this.fuelname = this.totalNameData.map((item) => item.FuelName);
        this.totalamount = this.totalNameData.map((item) => item.TotalAmount);
        this.totalliters = this.totalNameData.map((item) => item.TotalLiters);
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
      complete: () => console.log("WebSocket connection closed"),
      error: err => {
        console.error(err);
      }
    })

    // ✅ Load chart line sum revenue name
    const defaultColors = ['#42A5F5', '#66BB6A', '#FFA726', '#EF5350', '#AB47BC', '#FFEB3B', '#26C6DA', '#8D6E63'];
    this.linecharsocket = webSocket<totalrevenue7day[]>(environment.wsServerURI + `/ws/sumrevenueday?token=${localStorage.getItem('jwt')}`);
    console.log("url: ", this.linecharsocket);
    this.linecharsocket.subscribe({
      next: res => {
        this.totalDate = res;
        console.log('Line Chart date Websocket connected');
        console.log("✔️ line data: ", res);
        const dateSet = new Set<string>();
        const stationMap = new Map<string, { [date: string]: number }>();
        this.totalDate.forEach(item => {
          if (!item.Date || !item.StationName) return; 
          const datePart = item.Date.split(' ')[0];
          const [month, day, year] = datePart.split('/');
          if (!month || !day || !year) return;
          const isoString = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
          dateSet.add(isoString);
          if (!stationMap.has(item.StationName)) {
            stationMap.set(item.StationName, {});
          }
          stationMap.get(item.StationName)![isoString] = item.TotalRevenue;
        });
        const sortedDates = Array.from(dateSet).sort((a, b) => new Date(a).getTime() - new Date(b).getTime());
        this.DataLable = sortedDates.map(date => new Date(date).toLocaleDateString('vi-VN'));
        console.log("data label: ", this.DataLable);
        const datasets = Array.from(stationMap.entries()).map(([station, values], index) => {
          const data = sortedDates.map(date => values[date] ?? null); 
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
        console.log("==> stationMap: ", stationMap);
      },
      complete: () => console.log("WebSocket connection closed"),
      error: err => console.error(err)
    });

    // ✅ Load chart bar y sum revenue by type
    this.barycharsocket = webSocket<totalLogType[]>(environment.wsServerURI + `/ws/sumrenuetype?token=${localStorage.getItem('jwt')}`);
    this.barycharsocket.subscribe({
      next: res => {
        this.totalLog = res
        // console.log('bar y Chart date Websocket connected');
        // console.log("✔️ bary data: ", res)
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
      },
      complete: () => console.log("WebSocket connection closed"),
      error: err => {
        console.error(err);
      }
    })
  }

  ngOnDestroy(): void {
    this.barycharsocket?.complete();
    this.linecharsocket?.complete();
    this.piecharsocket?.complete();
    this.revenuesocket?.complete();
    this.barsocket?.complete();
  }
}
