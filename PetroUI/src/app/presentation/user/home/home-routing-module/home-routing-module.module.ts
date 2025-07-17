import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportStationComponent } from '../report-station/report-station.component';

const routes: Routes = [
  // {
  //   path: "report/:id",
  //   title: "Report",
  //   loadComponent: () => import('../report-station/report-station.component').then(m=>m.ReportStationComponent)
  // },
  {
    path: "report/:id",
    title: "Report",
    component: ReportStationComponent
    // loadComponent: () => import('../report-station/report-station.component').then(m=>m.ReportStationComponent)
  },
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class HomeRoutingModule { }
