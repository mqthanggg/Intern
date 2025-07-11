
import { Component, OnInit} from '@angular/core';
import { CurrencyPipe, CommonModule } from '@angular/common';
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
  socket: WebSocket | undefined;

  ngOnInit(): void {
    this.connectWebSocket();
  }

  connectWebSocket(): void {
    const url = 'ws://localhost:5170/ws/revenue';
    this.socket = new WebSocket(url);
    this.socket.onopen = () => {
      console.log('WebSocket connected');
    };
    this.socket.onmessage = (event) => {
      const data = JSON.parse(event.data);
      console.log('Received data:', data);
      this.revenueData = data;
    };
    this.socket.onerror = (error) => {
      console.error('WebSocket error:', error);
    };
    this.socket.onclose = () => {
      console.warn('WebSocket closed');
    };
  }

  ngOnDestroy(): void {
    if (this.socket) {
      this.socket.close();
    }
  }
}