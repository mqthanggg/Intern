import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

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
    loadComponent: () => import('../home/home.component').then(m=>m.ReportComponent)
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
