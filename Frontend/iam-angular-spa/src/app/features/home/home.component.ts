import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
import {
  BillingApiService,
  getBillingApiErrorMessage
} from '../../core/billing/billing-api.service';
import { environment } from '../../../environments/environment';

type BillingViewState = 'not-configured' | 'loading' | 'error' | 'empty' | 'success';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="home-heading">
      <div>
        <div class="eyebrow"><span class="eyebrow-dot"></span> COMPANY WORKSPACE</div>
        <h1>Welcome, {{ auth.displayName }}</h1>
        <p>Your IAM authentication completed successfully. You are now signed in to the portal.</p>
      </div>
      <div class="signed-in-pill"><span></span> Authenticated</div>
    </section>

    <section class="dashboard-grid">
      <article class="dashboard-card profile-card">
        <div class="card-topline"><span class="tile-icon purple">◎</span><span class="tile-label">YOUR PROFILE</span></div>
        <h2>{{ auth.displayName }}</h2>
        <p class="profile-email">{{ auth.email }}</p>
        <div class="profile-separator"></div>
        <div class="detail-row"><span>Identity provider</span><strong>Company IAM</strong></div>
        <div class="detail-row"><span>Client application</span><strong>angular-web</strong></div>
        <div class="detail-row"><span>Authentication</span><strong class="success-text">Verified</strong></div>
      </article>

      <article class="dashboard-card access-card">
        <div class="card-topline"><span class="tile-icon blue">⌘</span><span class="tile-label">APPLICATION ACCESS</span></div>
        <h2>Connected workspace</h2>
        <p class="card-copy">This SPA requests identity, Billing read, and Payment read scopes. Pricing uses a separate public catalogue scope when enabled:</p>
        <div class="scope-chip-list">
          <span class="scope-chip">openid</span>
          <span class="scope-chip">profile</span>
          <span class="scope-chip">email</span>
          <span class="scope-chip">billing.read</span>
          <span class="scope-chip">payment.read</span>
        </div>
        <div class="info-note">Internal service scopes such as pricing.read and users.read are not requested by Angular.</div>
      </article>

      <article class="dashboard-card next-card">
        <div class="card-topline"><span class="tile-icon green">↗</span><span class="tile-label">SESSION</span></div>
        <h2>IAM session active</h2>
        <p class="card-copy">The access token is kept in this browser session and sent only to the configured Billing API by the billing client.</p>
        <a routerLink="/home" class="text-link">Portal home <span>→</span></a>
      </article>
    </section>

    <section id="billing" class="billing-panel" aria-labelledby="billing-title">
      <div class="billing-panel-heading">
        <div>
          <div class="card-topline billing-topline">
            <span class="tile-icon green">₹</span>
            <span class="tile-label">CONNECTED SERVICE</span>
          </div>
          <h2 id="billing-title">Billing API</h2>
          <p class="billing-description">Authenticated read request using your IAM access token and the <code>billing.read</code> scope.</p>
        </div>
        <button
          type="button"
          class="billing-refresh-button"
          (click)="loadBillingData()"
          [disabled]="!billingConfigured || billingState() === 'loading'">
          @if (billingState() === 'loading') {
            <span class="small-spinner" aria-hidden="true"></span> Loading
          } @else {
            Refresh data <span aria-hidden="true">↻</span>
          }
        </button>
      </div>

      @switch (billingState()) {
        @case ('not-configured') {
          <div class="billing-state billing-state-neutral" role="status">
            <strong>Billing API endpoint not configured</strong>
            <p>Set <code>billing.apiBaseUrl</code> and <code>billing.readEndpoint</code> in <code>src/environments/environment.ts</code> after checking the Billing API project. No request has been sent and no endpoint has been assumed.</p>
          </div>
        }
        @case ('loading') {
          <div class="billing-state" role="status">
            <span class="spinner billing-spinner" aria-hidden="true"></span>
            <strong>Loading Billing data</strong>
            <p>Calling the configured Billing API with your IAM bearer token…</p>
          </div>
        }
        @case ('error') {
          <div class="billing-state billing-state-error" role="alert">
            <strong>Billing request failed</strong>
            <p>{{ billingError() }}</p>
            <button type="button" class="billing-retry-button" (click)="loadBillingData()">Try again</button>
          </div>
        }
        @case ('empty') {
          <div class="billing-state billing-state-neutral" role="status">
            <strong>Billing API responded successfully</strong>
            <p>The configured GET endpoint returned an empty response. There is currently no data to display.</p>
          </div>
        }
        @case ('success') {
          <div class="billing-success-heading" role="status">
            <span class="billing-success-dot"></span>
            <strong>Billing API response received</strong>
          </div>
          <p class="billing-description">The response is shown as JSON until the Billing API's actual DTO and business fields are available.</p>
          <pre class="billing-json"><code>{{ billingResponseJson }}</code></pre>
        }
      }
    </section>
  `
})
export class HomeComponent implements OnInit {
  readonly auth = inject(AuthService);
  private readonly billingApi = inject(BillingApiService);

  readonly billingConfigured = Boolean(
    environment.billing.apiBaseUrl.trim() && environment.billing.readEndpoint.trim()
  );
  readonly billingState = signal<BillingViewState>('not-configured');
  readonly billingError = signal<string | null>(null);
  private readonly billingData = signal<unknown>(null);

  get billingResponseJson(): string {
    return JSON.stringify(this.billingData(), null, 2) ?? 'null';
  }

  ngOnInit(): void {
    if (this.billingConfigured) {
      this.loadBillingData();
    }
  }

  loadBillingData(): void {
    if (!this.billingConfigured || this.billingState() === 'loading') {
      return;
    }

    this.billingState.set('loading');
    this.billingError.set(null);

    // Use `unknown` until the Billing API's real response contract is supplied.
    this.billingApi.get<unknown>(environment.billing.readEndpoint).subscribe({
      next: response => {
        this.billingData.set(response);
        this.billingState.set(this.isEmptyResponse(response) ? 'empty' : 'success');
      },
      error: error => {
        this.billingError.set(getBillingApiErrorMessage(error));
        this.billingState.set('error');
      }
    });
  }

  private isEmptyResponse(value: unknown): boolean {
    if (value === null || value === undefined) return true;
    if (typeof value === 'string') return value.trim().length === 0;
    if (Array.isArray(value)) return value.length === 0;
    if (typeof value === 'object') return Object.keys(value).length === 0;
    return false;
  }
}
