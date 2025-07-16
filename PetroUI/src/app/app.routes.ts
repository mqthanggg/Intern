import { Routes } from '@angular/router';
import { userGuard } from './infrastructure/guards/user-guard.guard';

export const routes: Routes = [
    {
        path: "login",
        title: "Login",
        loadComponent: () => import('./presentation/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: "error",
        title: "Error",
        loadComponent: () => import('./presentation/error/error.component').then(m => m.ErrorComponent)  
    },
    {
        path: "user",
        title: "User",
        canActivateChild: [userGuard],
        canActivate: [userGuard],
        loadComponent: ()=>import('./presentation/user/user.component').then(m => m.UserComponent),
        loadChildren: () => import('./presentation/user/user-routing-module/user-routing-module.module').then(m=>m.UserRoutingModule)
    },
    {
        path: "home",
        pathMatch: "full",
        redirectTo: "/user/home",
    },
    {
        path: "**",
        pathMatch: "full",
        redirectTo: "/error"
    }
];
