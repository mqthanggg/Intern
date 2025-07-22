import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportStationComponent } from '../home/report-station/report-station.component';
import { HomeComponent } from '../home/home.component';
import { StationComponent } from '../station/station.component';
import { StationsComponent } from '../stations/stations.component';
import { ReportComponent } from '../home/report-station/my-dialog/my-dialog.component';

const routes: Routes = [
  {
    path: "stations",
    title: "Stations",
    component: StationsComponent,
    loadChildren: () => import('../stations/stations-routing/stations-routing.module').then(m => m.StationsRoutingModule)
  },
  {
    path: "station/:id",
    title: "Station",
    component: StationComponent
  },
  {
    path: "home",
    title: "Home",
    component: HomeComponent
   },
  {
      path: "home/report/:id",
      title: "Report",
      component: ReportStationComponent
    },
{
      path: "home/report/station/:id",
      title: "Report-Station",
      component: ReportComponent
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
