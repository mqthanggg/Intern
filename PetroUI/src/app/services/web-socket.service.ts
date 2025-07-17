import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class WebSocketService {
  private sockets: { [key: string]: WebSocket } = {};

  connect(key: string, url: string): void {
    if (!this.sockets[key] || this.sockets[key].readyState === WebSocket.CLOSED) {
      this.sockets[key] = new WebSocket(url);

      this.sockets[key].onopen = () => console.log(`[${key}] Connected`);
      this.sockets[key].onclose = () => console.warn(`[${key}] Closed`);
      this.sockets[key].onerror = err => console.error(`[${key}] Error`, err);
    }
  }

  onMessage(key: string, callback: (data: any) => void): void {
    if (this.sockets[key]) {
      this.sockets[key].onmessage = (event) => {
        callback(JSON.parse(event.data));
      };
    }
  }

  send(key: string, data: any): void {
    if (this.sockets[key]?.readyState === WebSocket.OPEN) {
      this.sockets[key].send(JSON.stringify(data));
    }
  }

  close(key: string): void {
    if (this.sockets[key]) {
      this.sockets[key].close();
      delete this.sockets[key];
    }
  }

  closeAll(): void {
    for (const key in this.sockets) {
      this.sockets[key].close();
    }
    this.sockets = {};
  }
}
