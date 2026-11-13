import { ApplicationConfig, inject, provideAppInitializer } from '@angular/core';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { provideRouter, withInMemoryScrolling } from '@angular/router';
import { OAuthStorage, provideOAuthClient } from 'angular-oauth2-oidc';
import { routes } from './app.routes';
import { AuthService } from './core/auth/auth.service';

function sessionStorageFactory(): OAuthStorage {
  return window.sessionStorage;
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withInMemoryScrolling({ scrollPositionRestoration: 'top' })),
    provideHttpClient(withInterceptorsFromDi()),
    provideOAuthClient(),
    { provide: OAuthStorage, useFactory: sessionStorageFactory },
    provideAppInitializer(() => inject(AuthService).initialize())
  ]
};
