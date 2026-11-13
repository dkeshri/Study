import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { environment } from '../../../environments/environment';
import { PaymentApiService } from '../../core/payment/payment-api.service';
import { PricingApiService } from '../../core/pricing/pricing-api.service';
import { getAuthenticatedApiErrorMessage } from '../../core/api/authenticated-api-client.service';

type ApiResourceKey = 'pricing' | 'payment';
type ApiViewState = 'scope-not-configured' | 'not-configured' | 'loading' | 'error' | 'empty' | 'success';

interface ApiResourcePageMetadata {
  key: ApiResourceKey;
  title: string;
  eyebrow: string;
  description: string;
  requiredScope: string;
  expectedAudience: string;
  configInstruction: string;
  notConfiguredMessage: string;
  scopeInstruction: string;
}

const PAGE_METADATA: Record<ApiResourceKey, ApiResourcePageMetadata> = {
  pricing: {
    key: 'pricing',
    title: 'Pricing',
    eyebrow: 'PRICING WORKSPACE',
    description: 'View pricing information from the Pricing API using a user-authorized IAM access token.',
    requiredScope: 'pricing.read',
    expectedAudience: 'pricing-api',
    configInstruction: 'Set pricing.apiBaseUrl and pricing.readEndpoint in src/environments/environment.ts after checking the Pricing API project.',
    notConfiguredMessage: 'The Pricing API address or GET route has not been configured. No request has been sent.',
    scopeInstruction: 'The browser client must request the dedicated pricing.catalog.read scope. Do not expose the internal pricing.read service-to-service scope to Angular.'
  },
  payment: {
    key: 'payment',
    title: 'Payments',
    eyebrow: 'PAYMENT WORKSPACE',
    description: 'View payment information from the Payment API using your authenticated IAM session.',
    requiredScope: 'payment.read',
    expectedAudience: 'payment-api',
    configInstruction: 'Set payment.apiBaseUrl and payment.readEndpoint in src/environments/environment.ts after checking the Payment API project.',
    notConfiguredMessage: 'The Payment API address or GET route has not been configured. No request has been sent.',
    scopeInstruction: 'The Angular client must request payment.read, and the Payment API must enforce the same scope and payment-api audience.'
  }
};

@Component({
  selector: 'app-api-resource-page',
  standalone: true,
  imports: [RouterLink],
  template: `
    <section class="home-heading resource-heading">
      <div>
        <div class="eyebrow"><span class="eyebrow-dot"></span> {{ metadata.eyebrow }}</div>
        <h1>{{ metadata.title }}</h1>
        <p>{{ metadata.description }}</p>
      </div>
      <div class="signed-in-pill"><span></span> Authenticated</div>
    </section>

    <section class="billing-panel resource-panel" [attr.aria-labelledby]="'resource-title'">
      <div class="billing-panel-heading">
        <div>
          <div class="card-topline billing-topline">
            <span class="tile-icon" [class.blue]="metadata.key === 'pricing'" [class.green]="metadata.key === 'payment'">
              {{ metadata.key === 'pricing' ? '◇' : '₹' }}
            </span>
            <span class="tile-label">CONNECTED SERVICE</span>
          </div>
          <h2 id="resource-title">{{ metadata.title }} API</h2>
          <p class="billing-description">
            Required scope: <code>{{ metadata.requiredScope }}</code>
            · Expected audience: <code>{{ metadata.expectedAudience }}</code>
          </p>
        </div>
        <button
          type="button"
          class="billing-refresh-button"
          (click)="loadData()"
          [disabled]="!canLoad || state() === 'loading'">
          @if (state() === 'loading') {
            <span class="small-spinner" aria-hidden="true"></span> Loading
          } @else {
            Refresh data <span aria-hidden="true">↻</span>
          }
        </button>
      </div>

      @switch (state()) {
        @case ('scope-not-configured') {
          <div class="billing-state billing-state-neutral" role="status">
            <strong>Public API scope not enabled in Angular</strong>
            <p>{{ metadata.scopeInstruction }}</p>
            <p>After registering the scope and granting it to angular-web in IAM, add <code>{{ metadata.requiredScope }}</code> to <code>iam.scope</code> in <code>src/environments/environment.ts</code>, then sign in again.</p>
          </div>
        }
        @case ('not-configured') {
          <div class="billing-state billing-state-neutral" role="status">
            <strong>{{ metadata.title }} API endpoint not configured</strong>
            <p>{{ metadata.notConfiguredMessage }} {{ metadata.configInstruction }}</p>
          </div>
        }
        @case ('loading') {
          <div class="billing-state" role="status">
            <span class="spinner billing-spinner" aria-hidden="true"></span>
            <strong>Loading {{ metadata.title }} data</strong>
            <p>Calling the configured {{ metadata.title }} API with your IAM bearer token…</p>
          </div>
        }
        @case ('error') {
          <div class="billing-state billing-state-error" role="alert">
            <strong>{{ metadata.title }} request failed</strong>
            <p>{{ errorMessage() }}</p>
            <button type="button" class="billing-retry-button" (click)="loadData()">Try again</button>
          </div>
        }
        @case ('empty') {
          <div class="billing-state billing-state-neutral" role="status">
            <strong>{{ metadata.title }} API responded successfully</strong>
            <p>The configured GET endpoint returned an empty response. There is currently no data to display.</p>
          </div>
        }
        @case ('success') {
          <div class="billing-success-heading" role="status">
            <span class="billing-success-dot"></span>
            <strong>{{ metadata.title }} API response received</strong>
          </div>
          <p class="billing-description">The response is displayed as JSON until the API's actual response DTO is available.</p>
          <pre class="billing-json"><code>{{ responseJson }}</code></pre>
        }
      }
    </section>

    <nav class="resource-shortcuts" aria-label="Other workspace pages">
      <a routerLink="/home">← Home and Billing</a>
      @if (metadata.key !== 'pricing') { <a routerLink="/pricing">Pricing →</a> }
      @if (metadata.key !== 'payment') { <a routerLink="/payment">Payments →</a> }
    </nav>
  `
})
export class ApiResourcePageComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly pricingApi = inject(PricingApiService);
  private readonly paymentApi = inject(PaymentApiService);

  readonly metadata: ApiResourcePageMetadata = PAGE_METADATA[
    this.route.snapshot.data['apiResource'] as ApiResourceKey
  ];
  readonly state = signal<ApiViewState>('not-configured');
  readonly errorMessage = signal<string | null>(null);
  private readonly response = signal<unknown>(null);

  get responseJson(): string {
    return JSON.stringify(this.response(), null, 2) ?? 'null';
  }

  get apiConfigured(): boolean {
    const config = environment[this.metadata.key];
    return Boolean(config.apiBaseUrl.trim() && config.readEndpoint.trim());
  }

  get scopeConfigured(): boolean {
    return environment.iam.scope.split(/\s+/).includes(this.metadata.requiredScope);
  }

  get canLoad(): boolean {
    return this.apiConfigured && this.scopeConfigured;
  }

  ngOnInit(): void {
    if (!this.scopeConfigured) {
      this.state.set('scope-not-configured');
      return;
    }

    if (!this.apiConfigured) {
      this.state.set('not-configured');
      return;
    }

    this.loadData();
  }

  loadData(): void {
    if (!this.canLoad || this.state() === 'loading') return;

    this.state.set('loading');
    this.errorMessage.set(null);

    const request = this.metadata.key === 'pricing'
      ? this.pricingApi.get<unknown>()
      : this.paymentApi.get<unknown>();

    request.subscribe({
      next: value => {
        this.response.set(value);
        this.state.set(this.isEmptyResponse(value) ? 'empty' : 'success');
      },
      error: error => {
        this.errorMessage.set(getAuthenticatedApiErrorMessage(error, {
          displayName: this.metadata.title,
          requiredScope: this.metadata.requiredScope,
          expectedAudience: this.metadata.expectedAudience
        }));
        this.state.set('error');
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
