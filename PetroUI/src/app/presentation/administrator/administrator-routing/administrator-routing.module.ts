import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "attendance",
    title: "Attendance",
    loadComponent: () => import('../attendance/attendance.component').then(m=>m.AttendanceComponent),
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
