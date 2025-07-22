import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportComponent } from '../my-dialog/my-dialog.component';

const routes: Routes = [
  {
    path: "report/:id",
    title: "Report station",
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
export class StationsRoutingModule { }
