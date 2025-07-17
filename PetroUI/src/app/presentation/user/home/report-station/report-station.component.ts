import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { NgChartsModule } from 'ng2-charts';
import { FormControl, ReactiveFormsModule, UntypedFormGroup, Validators } from '@angular/forms';
import { WebSocketService } from './../../../../services/web-socket.service';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { catchError, delay, finalize, mergeMap, of, throwError } from 'rxjs';
import { CommonModule } from '@angular/common';
@Component({
    standalone: true,
    selector: 'app-report-station',
    templateUrl: './report-station.component.html',
    imports: [NgChartsModule, ReactiveFormsModule, CommonModule]
})
export class ReportStationComponent implements OnInit {
    loading = true;
    reportData: any;
    @Input('id') stationId = -1;
    lineChartData: any;
    lineChartOptions: any = {
        responsive: true,
        tension: 0.4,
        plugins: { legend: { display: true } }
    };
    isUpdateLoading = false
    stationForm = new UntypedFormGroup({
        name: new FormControl({ value: '', disabled: true }, [Validators.required]),
        address: new FormControl({ value: '', disabled: true }, [Validators.required])
    })
    constructor(
        private router: Router,
        private wsService: WebSocketService, 
        private http: HttpClient
    ) { }
    navigateBack(stationId: string) {
        // this.router.navigate([`/user/home/report/${stationId}`]);
    }
    ngOnInit(): void {
        this.isUpdateLoading = true
            this.http.put(`${environment.serverURI}/station/${this.stationId}`,this.stationForm.getRawValue(),{
              observe: 'response'
            }).pipe(
              mergeMap((val) => of(val).pipe(delay(1000))),
              catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),
              finalize(() => {this.isUpdateLoading = false})
            ).subscribe({
              next: (val) => {
                if (val.status === 200){
                  // this.router.navigate(['/user/home/report-station/']).then((_) => {
                  //   window.location.reload()
                  // })
                }
              },
              error: (err: HttpErrorResponse) => {
                console.error(err);
              }
            }
        )
        // Lấy stationId từ URL

        // Đăng ký lắng nghe dữ liệu WebSocket
        this.wsService.onMessage('stationData', (data: any) => {
            const stationData = data.find((d: any) => d.StationId === this.stationId);
            if (stationData) {
                this.reportData = {
                    name: stationData.StationName,
                    address: stationData.Address || 'Chưa có địa chỉ',
                    totalAmount: stationData.TotalRevenue,
                    totalLiters: stationData.TotalLiters || 0
                };

                this.lineChartData = {
                    labels: stationData.Dates || ['10/07', '11/07', '12/07'],
                    datasets: [
                        {
                            label: 'Doanh thu theo ngày',
                            data: stationData.DailyRevenue || [],
                            borderColor: '#42A5F5',
                            fill: false
                        }
                    ]
                };

                this.loading = false;
            }
        });
    }
}
