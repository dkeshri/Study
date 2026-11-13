import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';

@Component({
  selector: 'app-auth-callback',
  standalone: true,
  template: `
    <section class="callback-card">
      <div class="spinner" aria-hidden="true"></div>
      <h2>Completing sign-in</h2>
      <p>Validating your IAM response and opening your workspace…</p>
    </section>
  `
})
export class AuthCallbackComponent implements OnInit {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  ngOnInit(): void {
    if (this.auth.isAuthenticated) {
      void this.router.navigateByUrl(this.auth.consumeReturnUrl(), { replaceUrl: true });
      return;
    }

    void this.router.navigate(['/login'], {
      replaceUrl: true,
      queryParams: { authError: 'authorization_failed' }
    });
  }
}
