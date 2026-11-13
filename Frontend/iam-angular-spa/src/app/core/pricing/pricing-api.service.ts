import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticatedApiClient } from '../api/authenticated-api-client.service';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PricingApiService {
  private readonly api = inject(AuthenticatedApiClient);

  get<TResponse>(): Observable<TResponse> {
    return this.api.get<TResponse>(environment.pricing, environment.pricing.readEndpoint, 'Pricing');
  }
}
