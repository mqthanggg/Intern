import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "stations",
    title: "Stations",
    loadComponent: () => import('../stations/stations.component').then(m => m.StationsComponent),
  },
  {
    path: "account",
    title: "Account",
    loadComponent: () => import('../account/account.component').then(m => m.AccountComponent)
  },
  {
    path: "stations/attendance/:id",
    title: "Attendance",
    loadComponent: () => import('../stations/attendance/attendance.component').then(m => m.AttendanceComponent),
  },
  {
    path: "stations/assignment/:id",
    title: "Assignment",
    loadComponent: () => import('../stations/assignment/assignment.component').then(m => m.AssignmentComponent),
    loadChildren: () => import('../stations/assignment/assignment-routing/assignment-routing.module').then(m => m.AssignmentRoutingModule)
  },
  {
    path: "",
    pathMatch: "full",
    redirectTo: "/administrator/stations"
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

export class AdministratorRoutingModule { }
