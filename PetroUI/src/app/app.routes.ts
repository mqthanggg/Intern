import { Routes } from '@angular/router';
import { userGuard } from './user-guard.guard';

export const routes: Routes = [
    {
        path: "login",
        title: "Login",
        loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
    },
    {
        path: "error",
        title: "Error",
        loadComponent: () => import('./error/error.component').then(m => m.ErrorComponent)  
    },
    {
        path: "user",
        title: "User",
        canActivateChild: [userGuard],
        canActivate: [userGuard],
        loadComponent: ()=>import('./user/user.component').then(m => m.UserComponent),
        loadChildren: () => import('./user/user-routing-module/user-routing-module.module').then(m=>m.UserRoutingModuleModule)
    },
    {
        path: "",
        pathMatch: "full",
        redirectTo: "/login"
    },
    {
        path: "**",
        pathMatch: "full",
        redirectTo: "/error"
    }
];
