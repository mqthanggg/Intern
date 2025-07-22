import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterStateSnapshot } from '@angular/router';
import { StaffRecord } from '../staff-record';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../../environments/environment';
import { FormControl, UntypedFormGroup } from '@angular/forms';
import { Location } from '@angular/common';
import { forkJoin, map, throwError } from 'rxjs';
import { AssignmentRecord } from '../assignment-record';

@Component({
  selector: 'app-assignment-edit',
  standalone: true,
  imports: [],
  templateUrl: './assignment-edit.component.html',
  styleUrl: './assignment-edit.component.css'
})
export class AssignmentEditComponent implements OnInit {
  monthText = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December'
  ]
  private routeSnapshot: RouterStateSnapshot;
  staffRecords: StaffRecord[] = [];
  date: number = 0;
  month: number = 0;
  year: number = 0;
  assignmentForm: UntypedFormGroup;
  constructor(
    private router:Router,
    private activatedRoute: ActivatedRoute,
    private http:HttpClient,
    private location: Location,
  ){
    this.routeSnapshot = this.router.routerState.snapshot
    this.assignmentForm = new UntypedFormGroup({
      morningStaffs: new FormControl<StaffRecord[]>([]),
      afternoonStaffs: new FormControl<StaffRecord[]>([]),
      midnightStaffs: new FormControl<StaffRecord[]>([])
    })
  }
  navigateBack(){
    this.location.back()
  }
  ngOnInit(): void {
    let stationId: number = 0;
    this.activatedRoute.parent?.params.
    pipe(
      map(res => {
        if (res['id'] == undefined)
          throwError(() => new Error('Parameter ID is not found'))
        return res
      })
    ).
    subscribe({
      next: res => {
        stationId = res['id']
      },
      error: err => {
        console.error(err);
        this.router.navigate(['/administrator/stations'])
      }
    })
    if(this.routeSnapshot.root.queryParams['date'] == undefined)
      this.router.navigate(['/administrator/stations'])
    else {
      [this.year, this.month, this.date] = (this.routeSnapshot.root.queryParams['date'] as string).split('-').map((val) => Number(val))
      forkJoin([
        this.http.get<StaffRecord[]>(
          environment.serverURI + '/staffs',
          {
            withCredentials: true,
            observe: 'response'
          }
        ),
        this.http.post<AssignmentRecord[]>(
          environment.serverURI + `/assignments/station`,
          {
            stationId: stationId,
            workDate: new Date(this.year,this.month,this.date).toISOString()
          },
          {
            withCredentials: true,
            observe: 'response'
          }
        )
      ]).subscribe({
        next: res => {
          this.staffRecords = res[0].body!
          console.log(res[1].body!);
          
          this.assignmentForm.get('morningStaffs')?.setValue(
            res[1].body!.filter(
              res => res.shiftType == 1
            )
          )
          this.assignmentForm.get('afternoonStaffs')?.setValue(
            res[1].body!.filter(
              res => res.shiftType == 2
            )
          )
          this.assignmentForm.get('midnightStaffs')?.setValue(
            res[1].body!.filter(
              res => res.shiftType == 3
            )
          )
        }
      })
      
    }
  }
}
