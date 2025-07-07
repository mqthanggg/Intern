import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})

export class TitleService {
  private title = new BehaviorSubject<string>('');
  public title$ = this.title.asObservable();
  updateTitle(name: string){
    this.title.next(name);
  }
   private apiUrl = 'http://localhost:5170/api/revenue/chart';  // Thay URL đúng của bạn

  constructor(private http: HttpClient) { }

  getRevenueChart(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
