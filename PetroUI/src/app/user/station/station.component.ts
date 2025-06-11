import { HttpClient, HttpResponse } from '@angular/common/http';
import { Component, Input, OnInit } from '@angular/core';
import { TitleService } from '../title.service';
import { environment } from '../../../environments/environment';
import { delay, mergeMap,catchError,finalize,of,throwError } from 'rxjs';
import { ElementDraggableDirective, ElementDraggableSectionDirective } from '../../element-draggable.directive';


@Component({
  selector: 'app-station',
  standalone: true,
  imports: [ElementDraggableDirective, ElementDraggableSectionDirective],
  templateUrl: './station.component.html',
  styleUrl: './station.component.css'
})
export class StationComponent implements OnInit{
  @Input() id: number = -1;
  stationName: string = "";
  stationAddress: string = "";
  isStationLoading = false;
  isDispenserLoading = false;
  constructor(private http: HttpClient, private titleServer: TitleService){}
  ngOnInit(): void {
    setTimeout(() => {
      this.isStationLoading = true
      this.http.get(environment.serverURI+`/station/${this.id}`,{observe: "response"}).pipe(
        mergeMap((res) => of(res).pipe(delay(1000))),//Simulating delay
        catchError((err) => of(err).pipe(delay(1000),mergeMap(() => throwError(() => err)))),//Simulating delay
        finalize(() => {
          this.isStationLoading = false
        })).subscribe({
          next: (res: HttpResponse<any>) => {
            this.titleServer.updateTitle(this.stationName)
          }
      })
    },0)
  }
}
