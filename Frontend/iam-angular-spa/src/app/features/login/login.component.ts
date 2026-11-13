import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="auth-layout">
      <div class="auth-intro">
        <div class="eyebrow"><span class="eyebrow-dot"></span> COMPANY IDENTITY</div>
        <h1>One secure sign-in.<br><span>Everything connected.</span></h1>
        <p class="intro-copy">Access your company workspace using the central Identity and Access Management service.</p>
        <div class="trust-row">
          <span class="trust-icon">✓</span>
          <span>Authentication handled by your IAM service</span>
        </div>
        <div class="trust-row">
          <span class="trust-icon">✓</span>
          <span>Single sign-on ready for connected services</span>
        </div>
      </div>

      <div class="auth-card">
        <div class="card-icon">↗</div>
        <p class="card-kicker">WELCOME BACK</p>
        <h2>Sign in to your account</h2>
        <p class="card-copy">Continue to the secure IAM sign-in page. Your password is entered only on the IAM domain.</p>

        @if (auth.initializationError) {
          <div class="alert alert-error" role="alert">{{ auth.initializationError }}</div>
        }

        <button class="primary-button" type="button" (click)="signIn()">
          Continue with IAM <span aria-hidden="true">→</span>
        </button>
        <div class="card-divider"><span>NEW TO THE PORTAL?</span></div>
        <a class="secondary-button" routerLink="/register">Create an account</a>
        <p class="security-note"><span>▣</span> You will be redirected to the trusted IAM sign-in page.</p>
      </div>
    </section>
  `
})
export class LoginComponent {
  readonly auth = inject(AuthService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  constructor() {
    if (this.auth.isAuthenticated) {
      void this.router.navigateByUrl('/home', { replaceUrl: true });
    }
  }

  signIn(): void {
    const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/home';
    this.auth.beginLogin(returnUrl);
  }
}
