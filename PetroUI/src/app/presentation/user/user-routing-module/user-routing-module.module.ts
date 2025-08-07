import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportStationComponent } from '../home/report-station/report-station.component';
import { HomeComponent } from '../home/home.component';
import { StationComponent } from '../station/station.component';
import { StationsComponent } from '../stations/stations.component';
import { ReportComponent } from '../home/report-station/date-dialog/date-dialog.component';
import { ReportMonthComponent } from '../home/report-station/month-dialog/month-dialog.component';
import { ReportYearComponent } from '../home/report-station/year-dialog/year-dialog.component';
import { StationLogComponent } from '../station/station-log/station-log.component';

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
    path: "station/log/:id",
    title: "Station-log",
    component: StationLogComponent
  },
  {
    path: "home",
    title: "Home",
    component: HomeComponent
  },
  {
    path: "home/report/:id",
    title: "Report",
    component: ReportStationComponent,
    children: [
      {
        path: "day/:date",
        title: "Report-Day-Station",
        component: ReportComponent
      },
      {
        path: "month/:month",
        title: "Report-Month-Station",
        component: ReportMonthComponent
      },
      {
        path: "year/:year",
        title: "Report-Year-Station",
        component: ReportYearComponent
      },
    ]
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
