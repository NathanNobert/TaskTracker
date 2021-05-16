import { RouterModule, Routes } from "@angular/router";
import { HomeComponent } from "./pages/home/home.component";
import { SystemErrorComponent } from "./pages/system-error/system-error.component";
import { ModuleWithProviders } from "@angular/core";
import { LoginComponent } from "./pages/login/login.component";

const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'home', component: HomeComponent, pathMatch: 'full' },
            { path: 'login', component: LoginComponent, pathMatch: 'full' },
        ]
    },
    {
        path: 'systemerror',
        children: [{ path: '', component: SystemErrorComponent }]
    },
    {
        path: '*', component: SystemErrorComponent
    }
];

export const routing: ModuleWithProviders<RouterModule> = RouterModule.forRoot(routes, { anchorScrolling: 'enabled' });
