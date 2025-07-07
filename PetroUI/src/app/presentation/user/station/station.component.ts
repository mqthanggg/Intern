import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { TitleService } from '../../../infrastructure/services/title.service';
import { environment } from './../../../../environments/environment';
import { delay, mergeMap, catchError, finalize, of, throwError, Observable } from 'rxjs';
import { ElementDraggableDirective, ElementDraggableSectionDirective } from './../../../shared/directives/element-draggable.directive';
import { ActivatedRoute } from '@angular/router';
import { DispenserRecord } from './dispenser-record';
import { TankRecord } from './tank-record';
import { NgClass } from '@angular/common';
import { LogRecord } from './log-record';
import { NgChartsModule } from 'ng2-charts';
import { SumRevenueByNameResponse } from './sum-revenue.model';
import { ChartData, ChartOptions, ChartConfiguration } from 'chart.js';
import { WebSocketSubject } from 'rxjs/webSocket';
@Component({
  selector: 'app-station',
  standalone: true,
  imports: [ElementDraggableDirective, ElementDraggableSectionDirective, NgClass, NgChartsModule],
  templateUrl: './station.component.html',
  styleUrls: ['./station.component.css'] // ✅ sửa từ styleUrl → styleUrls
})
export class StationComponent implements OnInit {
  @Input() id: number = -1;
  stationName: string = "";
  stationAddress: string = "";
  isDispenserLoading = false;
  isTankLoading = false;
  isLogLoading = false;
  dispenserList: DispenserRecord[] = [];
  tankList: TankRecord[] = [];
  logList: LogRecord[] = [];
  _temp_statusList: number[] = [];
  private socket!: WebSocketSubject<any>;

  public pieChartLabels: string[] = [] // ["A95", "E5", "DO"];
  public pieChartData: number[] = [] // [300, 200, 100];

  public pieChartType: any = 'pie';
  public pieChartOptions: ChartConfiguration<'pie'>['options'] = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
      }
    }
  }

  constructor(
    private http: HttpClient,
    private titleServer: TitleService,
    private route: ActivatedRoute,
    private TitleService: TitleService
  ) { }

  ngOnInit(): void {
    const snapshot = this.route.snapshot;
    this.stationName = snapshot.queryParams['name'];
    this.stationAddress = snapshot.queryParams['address'];
    setTimeout(() => {
      this.titleServer.updateTitle(this.stationName);

      // ✅ Load dispenser
      this.isDispenserLoading = true;
      this.http.get(environment.serverURI + `/dispenser/station/${this.id}`, { observe: "response" }).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),
        catchError((err) => of(err).pipe(delay(1000), mergeMap(() => throwError(() => err)))),
        finalize(() => {
          this.isDispenserLoading = false;
        })
      ).subscribe({
        next: (res: HttpResponse<any>) => {
          this.dispenserList = res.body;
        },
        error: (err: HttpErrorResponse) => {
          console.error('Lỗi dispenser:', err);
        }
      });

      // ✅ Load tank
      this.isTankLoading = true;
      this.http.get(environment.serverURI + `/tank/station/${this.id}`, { observe: "response" }).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),
        catchError((err) => of(err).pipe(delay(1000), mergeMap(() => throwError(() => err)))),
        finalize(() => {
          this.isTankLoading = false;
        })
      ).subscribe({
        next: (res: HttpResponse<any>) => {
          this.tankList = res.body;
        },
        error: (err: HttpErrorResponse) => {
          console.error('Lỗi tank:', err);
        }
      });

      // ✅ Load log
      this.isLogLoading = true;
      this.http.get(environment.serverURI + `/log/station/${this.id}`, { observe: "response" }).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),
        catchError((err) => of(err).pipe(delay(1000), mergeMap(() => throwError(() => err)))),
        finalize(() => {
          this.isLogLoading = false;
        })
      ).subscribe({
        next: (res: HttpResponse<any>) => {
          this.logList = res.body;
        },
        error: (err: HttpErrorResponse) => {
          console.error('Lỗi log:', err);
        }
      });
      //=======================================
      this.socket = new WebSocketSubject('ws://localhost:5170/ws/Total/total_revenue_by_name/' + this.id);
      this.socket.subscribe({
        next: (data) => {
          this.pieChartLabels = data.fuelName;
          this.pieChartData = data.TongDoanhThu;
        },
        error: err => console.error('WebSocket error:', err),
        complete: () => console.log('WebSocket closed'),
      });
    }, 0);
  }

  // ✅ Mô phỏng bơm xăng
  simulatingStatus: "IDLE" | "PUMP" | "RESET" = "IDLE";
  simulatingLiter = 0;
  simulatingTotalPrice = 0;

  simulation() {
    this.simulatingStatus = "IDLE";
    this._temp_statusList = new Array(this.tankList.length).fill(0).map(() => Math.round(Math.random() * 100) % 101);

    const update = () => {
      if (this.simulatingTotalPrice < 100000) {
        this.simulatingStatus = "PUMP";
        this.simulatingLiter += 0.1;
        this.simulatingLiter = Math.round(this.simulatingLiter * 10) / 10;
        this.simulatingTotalPrice += 50;
        requestAnimationFrame(update);
      } else {
        this.simulatingStatus = "RESET";
        setTimeout(() => {
          this.simulatingStatus = "IDLE";
          this.simulatingLiter = 0;
          this.simulatingTotalPrice = 0;
        }, 2000);
      }
    };
    update();
  }
}
