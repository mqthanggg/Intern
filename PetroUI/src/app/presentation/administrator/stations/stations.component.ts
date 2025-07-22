import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { StationRecord } from '../../user/stations/station-record';
import { environment } from '../../../../environments/environment';
import { RouterLink, RouterOutlet } from '@angular/router';
import { TitleService } from '../../../infrastructure/services/title.service';

@Component({
  selector: 'app-stations',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './stations.component.html',
  styleUrl: './stations.component.css'
})
export class StationsComponent implements OnInit {
  stationsRecord: StationRecord[] = []
  constructor(
    private http: HttpClient,
    private titleService: TitleService
  ){

  }
  ngOnInit(): void {
    setTimeout(() => {
      this.titleService.updateTitle('Stations')
    },0)
    this.http.get<StationRecord[]>(
      environment.serverURI + '/stations',
      {
        withCredentials: true,
        observe: 'response'
      }
    ).subscribe({
      next: res => {
        this.stationsRecord = res.body ?? []
      },
      error: err => {
        console.error(err);
      }
    })
  }
}
