
import { Component, OnInit} from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
@Component({
  selector: 'app-dashboard',
  standalone: true,
  templateUrl: './report.component.html',
  styleUrl: './report.component.css',
  imports: [
    CommonModule, 
    CurrencyPipe 
  ]
})

export class ReportComponent implements OnInit {
  constructor() { 
    
  }
  revenueData: any = {};
  stationData: any = {};
  revenuesocket: WebSocket | undefined;
  stationsocket: WebSocket | undefined;

  ngOnInit(): void {
    this.connectWebsocket();
  }

  connectWebsocket(): void {
    this.revenuesocket = new WebSocket(environment.wsServerURI + '/ws/revenue');
    this.stationsocket = new WebSocket(environment.wsServerURI + '/ws/station');

    this.revenuesocket.onopen = () => console.log('Revenue Websocket connected');
    this.revenuesocket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      console.log('Received revenue:', data);
      this.revenueData = data;
    };
    this.revenuesocket.onerror = (error) => console.error('Revenue Websocket error:', error);
    this.revenuesocket.onclose = () => console.warn('Revenue Websocket closed');


    this.stationsocket.onopen = () => console.log('Station Websocket connected');
    this.stationsocket.onmessage = (event) => {
      const SumStation = JSON.parse(event.data);
      console.log('Total stations:', SumStation);
      this.stationData = SumStation;
    };
    this.stationsocket.onerror = (error) => console.error('Station Websocket error:', error);
    this.stationsocket.onclose = () => console.warn('Station Websocket closed');

  }

  ngOnDestroy(): void {
    if (this.revenuesocket && this.stationsocket) {
      this.stationsocket.close();
      this.revenuesocket.close();
    }
  }
}