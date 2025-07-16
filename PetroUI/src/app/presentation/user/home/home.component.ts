import { Component, OnInit,ViewChild } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { ChartOptions, ChartDataset, ChartData } from 'chart.js';
import { Router } from '@angular/router';

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

export class ReportComponent implements OnInit {
  @ViewChild(NgChartsModule) chart: NgChartsModule | undefined;
  constructor(private router: Router) {   }
  revenueData: any = {};
  stationData: any = {};
  barData: any = {};
  lineChartData: any = {};
  pieChartAccountData: any = {};
  pieChartLittersData: any = {};
  baryData: any = {};

  revenuesocket: WebSocket | undefined;
  stationsocket: WebSocket | undefined;
  barsocket: WebSocket | undefined;
  piecharsocket: WebSocket | undefined;
  linecharsocket: WebSocket | undefined;
  barycharsocket: WebSocket | undefined;

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
  chartLabels: string[] = [];
  chartDataAccount: number[] = [];

  chartDataLitters: number[] = [];
  chartDataAccounts: number[] = [];

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
  chartlineLabels: string[] = [];
  chartlineDataAccounts: number[] = [];

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
  chartpiebarLabels: string[] = [];
  chartMultbarDataAccounts: number[] = [];
  chartMultibarDataVolumes: number[] = [];

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
  chartBaryLogName: string[] = [];
  chartBaryDataAccounts: number[] = [];

  onChartClick(event: any) {
    const activePoint = event?.active?.[0];
    if (activePoint) {
      const datasetIndex = activePoint.datasetIndex;
      const dataset = this.lineChartData.datasets[datasetIndex] as any;

      const stationId = dataset.stationId;
      const stationName = dataset.label;

      if (stationId) {
        this.router.navigate(['./detail', stationId], {
          queryParams: { name: stationName }
        });
      }
    }
  }
  //=====
  ngOnInit(): void {
    this.connectWebsocket();
  }

  connectWebsocket(): void {
    // ✅ Load sum revenue
    this.revenuesocket = new WebSocket(environment.wsServerURI + '/ws/revenue');
    this.revenuesocket.onopen = () => console.log('Revenue Websocket connected');
    this.revenuesocket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      console.log('Received revenue:', data);
      this.revenueData = data;
    };
    this.revenuesocket.onerror = (error) => console.error('Revenue Websocket error:', error);
    this.revenuesocket.onclose = () => console.warn('Revenue Websocket closed');

    // ✅ Load sum station
    this.stationsocket = new WebSocket(environment.wsServerURI + '/ws/station');
    this.stationsocket.onopen = () => console.log('Station Websocket connected');
    this.stationsocket.onmessage = (event) => {
      const SumStation = JSON.parse(event.data);
      console.log('Total stations:', SumStation);
      this.stationData = SumStation;
    };
    this.stationsocket.onerror = (error) => console.error('Station Websocket error:', error);
    this.stationsocket.onclose = () => console.warn('Station Websocket closed');

    // ✅ Load chart bar sum revenue and profit
    this.barsocket = new WebSocket(environment.wsServerURI + '/ws/sumrevenue');
    this.barsocket.onopen = () => console.log('Bar Chart Websocket connected');
    this.barsocket.onmessage = (event) => {
      const rawData = JSON.parse(event.data);
      console.log('Received revenue:', rawData);
      const filteredData = rawData.filter((item: any) =>
        item.TotalRevenue > 0 || item.TotalProfit > 0
      );
      this.barData = filteredData;
      const stations = filteredData.map((item: any) => item.StationName);
      const revenue = filteredData.map((item: any) => item.TotalRevenue);
      const profit = filteredData.map((item: any) => item.TotalProfit);
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

    };
    this.barsocket.onerror = err => console.error('WebSocket Error', err);

    // ✅ Load chart multi sum revenue name
    this.piecharsocket = new WebSocket(environment.wsServerURI + `/ws/sumrenuename`);
    this.piecharsocket.onmessage = (event) => {
     const data: {
        FuelName: string;
        TotalAmount: number,
        TotalLiters: number
      }[] = JSON.parse(event.data );
      console.log("WebSocket pie :", this.piecharsocket);
      console.log("Received pie:", data);

      this.chartpiebarLabels = data.map(item => item.FuelName);
      this.chartDataLitters = data.map(item => item.TotalLiters);
      this.chartDataAccounts = data.map(item => item.TotalAmount);
      this.pieChartAccountData = {
        labels: this.chartpiebarLabels,
        datasets: [{
          data: this.chartDataAccounts,
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
        labels: this.chartpiebarLabels,
        datasets: [{
          data: this.chartDataLitters,
          backgroundColor: [
            '#FF6384',
            '#36A2EB',
            '#FFCE56',
            '#4BC0C0',
            '#9966FF'
          ]
        }]
      };
    };

    // ✅ Load chart line sum revenue name
    this.linecharsocket = new WebSocket(environment.wsServerURI + `/ws/sumrevenueday`);
    const defaultColors = ['#42A5F5', '#66BB6A', '#FFA726', '#EF5350', '#AB47BC', '#FFEB3B', '#26C6DA', '#8D6E63'];
    this.linecharsocket.onmessage = (event) => {
      const rawData: {
        Date: string;
        StationName: string;
        TotalAmount: number;
      }[] = JSON.parse(event.data);
      console.log("WebSocket line:", this.linecharsocket);
      console.log("Received line:", rawData);

      const dateSet = new Set<string>();
      rawData.forEach(item => dateSet.add(item.Date));
      const sortedDates = Array.from(dateSet).sort();

      this.chartlineLabels = sortedDates.map(date =>
        new Date(date).toLocaleDateString('vi-VN')
      );
      const stationMap = new Map<string, { [date: string]: number }>();
      rawData.forEach(item => {
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
        labels: this.chartlineLabels,
        datasets
      };
    };

    // ✅ Load chart bar sum revenue by type
    this.barycharsocket = new WebSocket(environment.wsServerURI + '/ws/sumrenuetype');
    this.barycharsocket.onmessage = (event) => {
       const data: {
        LogTypeName: string;
        TotalAmount: number,
      }[] = JSON.parse(event.data );
      console.log('Bar y Chart Websocket connected');
      console.log("Received bar y:", data);
      
      this.chartBaryLogName = data.map(item => item.LogTypeName);
      this.chartBaryDataAccounts = data.map(item => item.TotalAmount);
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

    };
    this.barsocket.onerror = err => console.error('WebSocket Error', err);
  }

  ngOnDestroy(): void {
    if (this.revenuesocket && this.stationsocket && this.barsocket) {
      this.stationsocket.close();
      this.revenuesocket.close();
      this.barsocket?.close();
    }
  }
}
