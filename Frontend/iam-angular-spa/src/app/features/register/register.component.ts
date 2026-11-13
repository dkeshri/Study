import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="auth-layout register-layout">
      <div class="auth-intro">
        <div class="eyebrow"><span class="eyebrow-dot"></span> GET STARTED</div>
        <h1>Your workspace<br><span>starts here.</span></h1>
        <p class="intro-copy">Create your company account through the central IAM service. Your profile and credentials stay under IAM control.</p>
        <div class="trust-row"><span class="trust-icon">✓</span><span>One account for connected company applications</span></div>
        <div class="trust-row"><span class="trust-icon">✓</span><span>Profile information stored by IAM</span></div>
      </div>

      <div class="auth-card">
        <div class="card-icon card-icon-green">＋</div>
        <p class="card-kicker">ACCOUNT REGISTRATION</p>
        <h2>Create your account</h2>
        <p class="card-copy">Registration is hosted by IAM so passwords are never collected or handled by this Angular application.</p>

        <div class="steps-list">
          <div class="step-item"><span class="step-number">1</span><span>Continue to the IAM sign-in page.</span></div>
          <div class="step-item"><span class="step-number">2</span><span>Select <strong>Register</strong> on the IAM page.</span></div>
          <div class="step-item"><span class="step-number">3</span><span>Create your account, then sign in to return here.</span></div>
        </div>

        <button class="primary-button" type="button" (click)="register()">
          Continue to registration <span aria-hidden="true">→</span>
        </button>
        <a class="text-link centered-link" routerLink="/login">Already have an account? Sign in</a>
        <p class="security-note"><span>▣</span> Registration and password management are provided by IAM.</p>
      </div>
    </section>
  `
})
export class RegisterComponent {
  readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  constructor() {
    if (this.auth.isAuthenticated) {
      void this.router.navigateByUrl('/home', { replaceUrl: true });
    }
  }

  register(): void {
    // This starts /connect/authorize first. The IAM Login page's Register link
    // must preserve ReturnUrl so the original authorization request can resume.
    this.auth.beginRegistration();
  }
}
