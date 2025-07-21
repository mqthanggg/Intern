import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { FormControl, ReactiveFormsModule, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { catchError, delay, finalize, mergeMap, of, throwError } from 'rxjs';
import { environment } from '../../../../../../environments/environment';
import { webSocket, WebSocketSubject } from 'rxjs/webSocket';
import { revenuestation, WSrevenuestation } from '../model/sumrevenuestation-record';

@Component({
  selector: 'app-report-station-chart',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './my-dialog.component.html',
  //   styleUrl: './report-station.component.css'
})
export class ReportComponent implements OnInit {
  isOpenErrorModal = false
  @Input() id: number = -1;
  revstationSocket: { [key: string]: WebSocketSubject<WSrevenuestation> } = {}
  revenuesocket: WebSocketSubject<revenuestation[]> | undefined;
  revstation: revenuestation[] = [];
  DataAccount: number[] = [];
  DataLitters: number[] = [];
  DataProfit: number[] = [];
  TotalLitters = 0;
  TotalRevenue = 0;
  totalProfit = 0;
   isUpdateLoading = false
  // stationForm = new UntypedFormGroup({
  //   name: new FormControl({ value: '', disabled: true }, [Validators.required]),
  //   address: new FormControl({ value: '', disabled: true }, [Validators.required])
  // })
  constructor(private router: Router, private http: HttpClient) { }
  ngOnInit(): void {
    const routeSnapshot = this.router.routerState.snapshot.root
  }
  navigateBack() {
    this.router.navigate(['/user/home/report-station'])
  }
  formSubmit() {
    this.revenuesocket = webSocket<revenuestation[]>(environment.wsServerURI + `/ws/station/${this.id}`)
    this.revenuesocket.subscribe({
      next: res => {
        this.revstation = res;
        this.revstation.forEach((value, index) => {
          this.revstationSocket[value.StationId] = webSocket<WSrevenuestation>(environment.wsServerURI + `/ws/station/${this.id}?token=${localStorage.getItem('jwt')}`)
          this.revstationSocket[value.StationId].subscribe({
            next: (Datares: WSrevenuestation) => {
              this.revstation[index].TotalLiters = Datares.liter
              this.revstation[index].TotalRevenue = Datares.revenue
              this.revstation[index].TotalProfit = Datares.profit
            },
            error: (err) => {
              console.error(`Error at station ${value.StationId}: ${err}`);
            }
          })
        })
        this.DataAccount = this.revstation.map((item) => item.TotalRevenue);
        this.TotalRevenue = this.DataAccount.reduce((acc, val) => acc + val, 0);
        this.DataLitters = this.revstation.map((item) => item.TotalLiters);
        this.TotalLitters = this.DataLitters.reduce((acc, val) => acc + val, 0);
        this.DataProfit = this.revstation.map((item) => item.TotalProfit);
        this.totalProfit = this.DataProfit.reduce((acc, val) => acc + val, 0);
      },
      error: err => {
        console.error(err);
      }
    })
  }
}
