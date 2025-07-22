import { Routes } from '@angular/router';
import { userGuard } from './infrastructure/guards/user-guard.guard';
import { UserComponent } from './presentation/user/user.component';

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
        component: UserComponent,
        loadChildren: () => import('./presentation/user/user-routing-module/user-routing-module.module').then(m=>m.UserRoutingModule)
    },
    {
        path: "**",
        pathMatch: "full",
        redirectTo: "/error"
    }
];
