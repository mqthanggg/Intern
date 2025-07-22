import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StationDeleteComponent } from '../station-delete/station-delete.component';
import { StationEditComponent } from '../station-edit/station-edit.component';

const routes: Routes = [
  {
    path: "delete/:id",
    title: "Deleting station",
    component: StationDeleteComponent
  },
  {
    path: "edit/:id",
    title: "Editing station",
    component: StationEditComponent
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
export class StationsRoutingModule { }
