import { AsyncPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from './core/auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, AsyncPipe],
  template: `
    <div class="app-shell">
      <header class="topbar">
        <a class="brand" routerLink="/">
          <span class="brand-mark">C</span>
          <span>Company<span class="brand-light">Portal</span></span>
        </a>
        <nav class="topnav" aria-label="Main navigation">
          @if (auth.isAuthenticated$ | async) {
            <a routerLink="/home" class="nav-link">Home</a>
            <button type="button" class="nav-button" (click)="auth.logout()">Sign out</button>
          } @else {
            <a routerLink="/login" class="nav-link">Sign in</a>
            <a routerLink="/register" class="nav-link nav-link-accent">Create account</a>
          }
        </nav>
      </header>

      <main class="page-shell">
        <router-outlet />
      </main>

      <footer class="footer">
        <span>Company Portal</span>
        <span>Identity managed securely by IAM</span>
      </footer>
    </div>
  `
})
export class AppComponent {
  readonly auth = inject(AuthService);
}
