import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "delete/:id",
    title: "Deleting station",
    loadComponent: () => import('../station-delete/station-delete.component').then(m => m.StationDeleteComponent)
  },
  {
    path: "edit/:id",
    title: "Editing station",
    loadComponent: () => import('../station-edit/station-edit.component').then(m => m.StationEditComponent)
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
