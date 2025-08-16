import { Routes } from '@angular/router';
import { userGuard } from './infrastructure/guards/user-guard.guard';
import { administratorGuard } from './infrastructure/guards/administrator-guard.guard';
import { autoRedirection } from './infrastructure/services/auto-redirection.service';
import { UserComponent } from './presentation/user/user.component';
import { ErrorComponent } from './presentation/error/error.component';
import { LoginComponent } from './presentation/login/login.component';
import { AdministratorComponent } from './presentation/administrator/administrator.component';

export const routes: Routes = [
    {
        path: "login",
        title: "Login",
        component: LoginComponent
    },
    {
        path: "error",
        title: "Error",
        component: ErrorComponent
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
        path: "administrator",
        title: "Administrator",
        canActivateChild: [administratorGuard],
        canActivate: [administratorGuard],
        component: AdministratorComponent,
        loadChildren: () => import('./presentation/administrator/administrator-routing/administrator-routing.module').then(m=>m.AdministratorRoutingModule)
    },
    {
        path: "",
        pathMatch: "full",
        redirectTo: autoRedirection,
    },
    {
        path: "**",
        pathMatch: "full",
        redirectTo: "/error"
    }
];
