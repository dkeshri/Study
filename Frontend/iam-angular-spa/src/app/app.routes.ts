import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { AuthCallbackComponent } from './features/auth-callback/auth-callback.component';
import { HomeComponent } from './features/home/home.component';
import { LoginComponent } from './features/login/login.component';
import { RegisterComponent } from './features/register/register.component';
import { ApiResourcePageComponent } from './features/api-resource/api-resource-page.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'auth/callback', component: AuthCallbackComponent },
  { path: 'home', component: HomeComponent, canActivate: [authGuard] },
  { path: 'pricing', component: ApiResourcePageComponent, canActivate: [authGuard], data: { apiResource: 'pricing' } },
  { path: 'payment', component: ApiResourcePageComponent, canActivate: [authGuard], data: { apiResource: 'payment' } },
  { path: '**', redirectTo: 'home' }
];
