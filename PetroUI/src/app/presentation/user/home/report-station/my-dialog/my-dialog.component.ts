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
  @Input() date?: String;
   isUpdateLoading = false

  constructor(private router: Router, private http: HttpClient) { }
  ngOnInit(): void {
    const routeSnapshot = this.router.routerState.snapshot.root
  }
  navigateBack() {
    this.router.navigate(['/user/home/report-station'])
  }
}
