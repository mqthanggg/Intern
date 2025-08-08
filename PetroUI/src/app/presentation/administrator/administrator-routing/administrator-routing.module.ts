import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: "account",
    title: "Account",
    loadComponent: () => import('../account/account.component').then(m => m.AccountComponent)
  },
  {
    path: "",
    pathMatch: "full",
    redirectTo: "/administrator/account"
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
