import { Routes } from '@angular/router';
import { SigninComponent } from './features/signin/signin-component';
import { HomeComponent } from './features/home/home-component/home-component';
import { authGuard } from './core/guards/auth-guard';

export const routes: Routes = [
    {
        path: 'signin',
        component: SigninComponent
    },
    {
        path: '',
        component: HomeComponent,
        canActivate: [authGuard]
    }
];
