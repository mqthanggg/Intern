
import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet, RouterStateSnapshot } from '@angular/router';
import { TitleService } from '../../../../infrastructure/services/title.service';
import { FormControl, UntypedFormGroup } from '@angular/forms';
import { StaffRecord } from './staff-record';
import { HttpClient } from '@angular/common/http';
import { AssignmentRecord } from './assignment-record';

function dayOfDate(year: number, month: number, date: number){
  date += (month < 3 ? year-- : year - 2)
  return (Math.trunc(23*month/9)+date+4+Math.trunc(year/4)-Math.trunc(year/100)+Math.trunc(year/400)) % 7
}

@Component({
  selector: 'app-assignment',
  standalone: true,
  imports: [RouterLink,RouterOutlet],
  templateUrl: './assignment.component.html',
  styleUrl: './assignment.component.css'
})
export class AssignmentComponent implements OnInit{
  assignmentForm: UntypedFormGroup;
  private routerSnapshot: RouterStateSnapshot;
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
  nextMonth: number = -1;
  year: number = 0;
  startDate: number = 0;
  calendar: number[] = [];
  constructor(
    private router: Router,
    private titleService: TitleService,
    private http: HttpClient
  ){
    this.routerSnapshot = this.router.routerState.snapshot
    this.assignmentForm = new UntypedFormGroup({
      morningStaffs: new FormControl<StaffRecord[]>([]),
      afternoonStaffs: new FormControl<StaffRecord[]>([]),
      midnightStaffs: new FormControl<StaffRecord[]>([]),
    })
  }
  ngOnInit(): void {
    if (this.routerSnapshot.root.queryParams['name'] == undefined)
      this.router.navigate(['administrator/stations'])
    else{
      setTimeout(() => {
        this.titleService.updateTitle(this.routerSnapshot.root.queryParams['name'])
      })
      this.nextMonth = new Date().getMonth() + 1
      this.year = new Date().getFullYear() + (this.nextMonth >= 12 ? 1 : 0)
      let maxDateOfMonth: number;
      switch(this.nextMonth){
        case 1:
        case 3:
        case 5:
        case 7:
        case 8:
        case 10:
        case 12:
          maxDateOfMonth = 31
          break
        case 4:
        case 6:
        case 9:
        case 11:
          maxDateOfMonth = 30
          break
        case 2:
          maxDateOfMonth = (!(this.year % 400) || (!(this.year % 4) && this.year % 100) ? 29 : 28)
          break
        default:
          maxDateOfMonth = 0
          break
      }
      this.calendar = new Array(maxDateOfMonth).fill(0).map((_,idx) => {
        return idx+1
      })
      this.startDate = dayOfDate(this.year, new Date().getMonth() + 2,1)
    }
  }
}
