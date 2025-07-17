import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportStationComponent } from '../home/report-station/report-station.component';

const routes: Routes = [
  {
    path: "stations",
    title: "Stations",
    loadComponent: () => import('../stations/stations.component').then(m=>m.StationsComponent),
    loadChildren: () => import('../stations/stations-routing/stations-routing.module').then(m => m.StationsRoutingModule)
  },
  {
    path: "station/:id",
    title: "Station",
    loadComponent: () => import('../station/station.component').then(m=>m.StationComponent)
  },
  {
    path: "home",
    title: "Home",
    loadComponent: () => import('../home/home.component').then(m=>m.HomeComponent),
    },
  {
      path: "home/report/:id",
      title: "Report",
      component: ReportStationComponent
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
export class UserRoutingModule { }
