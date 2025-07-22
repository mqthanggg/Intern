import { Component, OnInit } from '@angular/core';
import { TitleService } from '../../../../infrastructure/services/title.service';
import { HttpClient } from '@angular/common/http';
import { Router, RouterStateSnapshot } from '@angular/router';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [],
  templateUrl: './attendance.component.html',
  styleUrl: './attendance.component.css'
})
export class AttendanceComponent implements OnInit{
  private routerSnapshot: RouterStateSnapshot;
  constructor(
    private titleService: TitleService,
    private http: HttpClient,
    private router: Router
  ){
    this.routerSnapshot = this.router.routerState.snapshot
  }
  ngOnInit(): void {
    if (this.routerSnapshot.root.queryParams['name'] == undefined)
      this.router.navigate(['/administrator/stations'])
    else{
      setTimeout(() => {
        this.titleService.updateTitle(this.routerSnapshot.root.queryParams['name'])
      },0) 
    }
  }
}
