import { Component, OnInit } from '@angular/core';
import { TitleService } from '../../../infrastructure/services/title.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [],
  templateUrl: './attendance.component.html',
  styleUrl: './attendance.component.css'
})
export class AttendanceComponent implements OnInit{
  constructor(
    private titleService: TitleService,
    private http: HttpClient
  ){}
  ngOnInit(): void {
    setTimeout(() => {
      this.titleService.updateTitle("Attendance")
    },0) 
  }
}
