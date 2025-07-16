import { Routes } from '@angular/router';
import { userGuard } from './infrastructure/guards/user-guard.guard';
import { administratorGuard } from './infrastructure/guards/administrator-guard.guard';

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
        path: "administrator",
        title: "Administrator",
        canActivateChild: [administratorGuard],
        canActivate: [administratorGuard],
        loadComponent: () => import('./presentation/administrator/administrator.component').then(m => m.AdministratorComponent),
        loadChildren: () => import('./presentation/administrator/administrator-routing/administrator-routing.module').then(m=>m.AdministratorRoutingModule)
    },
    {
        path: "",
        pathMatch: "full",
        redirectTo: "/user",
    },
    {
        path: "**",
        pathMatch: "full",
        redirectTo: "/error"
    }
];
