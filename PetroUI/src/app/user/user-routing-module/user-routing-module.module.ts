import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "stations",
    title: "Stations",
    loadComponent: () => import('../stations/stations.component').then(m=>m.StationsComponent)
  },
  {
    path: "station/:id",
    title: "Station",
    loadComponent: () => import('../station/station.component').then(m=>m.StationComponent)
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class UserRoutingModuleModule { }
