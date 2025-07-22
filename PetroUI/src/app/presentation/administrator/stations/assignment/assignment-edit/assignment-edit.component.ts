import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterStateSnapshot } from '@angular/router';
import { StaffRecord } from '../staff-record';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../../environments/environment';
import { FormControl, UntypedFormGroup } from '@angular/forms';
import { Location } from '@angular/common';
import { filter, forkJoin, from, map, merge, mergeAll, mergeMap, of, throwError, toArray } from 'rxjs';
import { AssignmentRecord } from '../assignment-record';
import { ShiftRecord } from '../shift-record';

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
        this.http.get<AssignmentRecord[]>(
          environment.serverURI + `/assignments/station/${stationId}`,
          {
            withCredentials: true,
            observe: 'response'
          }
        )
      ]).
      pipe(
        mergeMap((res) => {
          this.staffRecords = res[0].body ?? []
          return from(res[1].body!).pipe(
            mergeMap(assignment => this.http.get<ShiftRecord>(
              environment.serverURI + `/shift/${assignment.shiftId}`,
              {
                withCredentials: true,
                observe: 'response'
              }
            ).pipe(
              map(shift => {
                return {assignment: assignment,shift: shift.body?.shiftType}
              })
            ))
          )
        }),
        toArray()
      ).subscribe({
        next: res => {
          this.assignmentForm.get('morningStaffs')?.setValue(
            res.filter(
              res => res.shift == 1
            ).map(res => res.assignment)
          )
          this.assignmentForm.get('afternoonStaffs')?.setValue(
            res.filter(
              res => res.shift == 2
            ).map(res => res.assignment)
          )
          this.assignmentForm.get('midnightStaffs')?.setValue(
            res.filter(
              res => res.shift == 3
            ).map(res => res.assignment)
          )
        }
      })
      
    }
  }
}
