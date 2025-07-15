import { Component, OnInit } from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { NgChartsModule } from 'ng2-charts';
import { ChartOptions, ChartDataset, ChartConfiguration } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './report.component.html',
  styleUrl: './report.component.css',
  imports: [
    CommonModule,
    CurrencyPipe,
    NgChartsModule
  ]
})

export class ReportComponent implements OnInit {
  constructor() {

  }

  
  revenueData: any = {};
  stationData: any = {};
  barData: any = {};
  revenueChartData:any={};
  revenuesocket: WebSocket | undefined;
  stationsocket: WebSocket | undefined;
  barsocket: WebSocket | undefined;
  piesocket: WebSocket | undefined;
  public barChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    animation: {
      duration: 0
    },
    plugins: {
      legend: { display: true },
      tooltip: {
        enabled: true
      }
    },
    onClick: (event: any, activeElements: any[]) => {
      if (activeElements.length > 0) {
        const chart = activeElements[0].element.$context.chart;
        const datasetIndex = activeElements[0].datasetIndex;
        const index = activeElements[0].index;

        const label = chart.data.labels[index];
        const value = chart.data.datasets[datasetIndex].data[index];
        const datasetLabel = chart.data.datasets[datasetIndex].label;
        alert(`Trạm: ${label}\n${datasetLabel}: ${value.toLocaleString()} VND`);
      }
    }
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
  public pieChartOptions: ChartConfiguration<'pie'>['options'] = {
    responsive: true,
    animation: false,
    plugins: {
      legend: {
        display: true,
        position: 'bottom' as const,
      }
    }
  };
  chartLabels: string[] = [];
  chartDataAccount: number[] = [];
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

    // ✅ Load chart pie sum revenue type
    this.piesocket = new WebSocket(environment.wsServerURI + `/ws/type`);
    this.piesocket.onmessage = (event) => {
      const data: { 
        LogTypeName: string; 
        TotalAmount: number,
        TotalLiters: number
      }[] = JSON.parse(event.data);
      console.log("WebSocket readyState:", this.piesocket);
      console.log("Received data:", data);

      this.chartLabels = data.map(item => item.LogTypeName);
      this.chartDataAccount = data.map(item => item.TotalAmount);
      this.revenueChartData = {
        labels: this.chartLabels,
        datasets: [{
          data: this.chartDataAccount,
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
  }

  ngOnDestroy(): void {
    if (this.revenuesocket && this.stationsocket && this.barsocket) {
      this.stationsocket.close();
      this.revenuesocket.close();
      this.barsocket?.close();
    }
  }
}
