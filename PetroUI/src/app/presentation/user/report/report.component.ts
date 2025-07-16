
import { Component, OnInit} from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { forkJoin } from 'rxjs';
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
  revenueSocket: WebSocketSubject<any> | undefined;
  stationSocket: WebSocketSubject<any> | undefined;

  ngOnInit(): void {
    this.connectWebsocket();
  }

  connectWebsocket(): void {
    this.revenueSocket = webSocket(environment.wsServerURI + '/ws/revenue');
    this.stationSocket = webSocket(environment.wsServerURI + '/ws/station');
    forkJoin([
      this.revenueSocket,
      this.stationSocket
    ]).subscribe({
      next: ([revenue,station]) => {
        this.revenueData = revenue;
        this.stationData = station;
      },
      error: (error) => console.error('Websocket error:', error),
      complete: () => console.warn('Websocket closed')
    })
  }

  ngOnDestroy(): void {
    this.stationSocket?.complete();
    this.revenueSocket?.complete();
  }
}