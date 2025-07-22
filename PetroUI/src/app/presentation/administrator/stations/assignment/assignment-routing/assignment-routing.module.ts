import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: "edit",
    title: "Assignment edit",
    loadComponent: () => import('../assignment-edit/assignment-edit.component').then(m => m.AssignmentEditComponent)
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
export class AssignmentRoutingModule { }
