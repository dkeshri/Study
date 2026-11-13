import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { environment } from '../../../environments/environment';

/**
 * A configuration error is raised before an HTTP request is sent. The API URL
 * and endpoint are deliberately not guessed: they must come from the Billing
 * API's actual deployment settings and route contract.
 */
export class BillingApiConfigurationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'BillingApiConfigurationError';
  }
}

@Injectable({ providedIn: 'root' })
export class BillingApiService {
  private readonly http = inject(HttpClient);
  private readonly auth = inject(AuthService);

  /**
   * Performs an authenticated GET against the configured Billing API.
   *
   * The caller supplies its real response DTO as TResponse when the API
   * contract is known. Until then, callers should use `unknown` rather than
   * pretending that an undocumented response has a particular shape.
   *
   * The bearer token is attached only here, to the configured Billing API
   * origin; it is not installed as a global interceptor for arbitrary hosts.
   */
  get<TResponse>(relativeEndpoint: string): Observable<TResponse> {
    let url: string;

    try {
      url = this.buildUrl(relativeEndpoint);
    } catch (error: unknown) {
      const message = error instanceof Error
        ? error.message
        : 'Billing API configuration is invalid.';
      return throwError(() => new BillingApiConfigurationError(message));
    }

    const accessToken = this.auth.accessToken;
    if (!accessToken) {
      return throwError(() => new BillingApiConfigurationError(
        'No IAM access token is available. Sign in again before calling the Billing API.'
      ));
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${accessToken}`,
      Accept: 'application/json'
    });

    return this.http.get<TResponse>(url, { headers });
  }

  private buildUrl(relativeEndpoint: string): string {
    const configuredBaseUrl = environment.billing.apiBaseUrl.trim();
    const endpoint = relativeEndpoint.trim();

    if (!configuredBaseUrl) {
      throw new BillingApiConfigurationError(
        'Set billing.apiBaseUrl in src/environments/environment.ts to your Billing API origin.'
      );
    }

    if (!endpoint) {
      throw new BillingApiConfigurationError(
        'Set billing.readEndpoint to an actual GET route exposed by your Billing API.'
      );
    }

    // Only a relative path is accepted. This prevents callers from choosing a
    // different host and receiving the IAM bearer token.
    if (/^[a-z][a-z\d+.-]*:/i.test(endpoint) || endpoint.startsWith('//')) {
      throw new BillingApiConfigurationError(
        'Billing API endpoint must be a relative path, not an absolute URL.'
      );
    }

    let base: URL;
    try {
      base = new URL(configuredBaseUrl);
    } catch {
      throw new BillingApiConfigurationError(
        'billing.apiBaseUrl must be a valid absolute URL, such as https://localhost:7002.'
      );
    }

    if (base.username || base.password) {
      throw new BillingApiConfigurationError(
        'Do not embed credentials in billing.apiBaseUrl.'
      );
    }

    if (base.protocol !== 'https:') {
      throw new BillingApiConfigurationError(
        'Billing API must use HTTPS, including for local development with the ASP.NET Core development certificate.'
      );
    }

    base.search = '';
    base.hash = '';
    if (!base.pathname.endsWith('/')) {
      base.pathname += '/';
    }

    const normalizedEndpoint = endpoint.replace(/^\/+/, '');
    const resolved = new URL(normalizedEndpoint, base);

    if (resolved.origin !== base.origin) {
      throw new BillingApiConfigurationError(
        'Billing API endpoint resolved outside the configured API origin.'
      );
    }

    return resolved.toString();
  }
}

/** Converts HTTP failures into user-safe messages without exposing response bodies or tokens. */
export function getBillingApiErrorMessage(error: unknown): string {
  if (error instanceof BillingApiConfigurationError) {
    return error.message;
  }

  if (!(error instanceof HttpErrorResponse)) {
    return 'An unexpected error occurred while contacting the Billing API.';
  }

  switch (error.status) {
    case 0:
      return 'The Billing API could not be reached. Check its URL, HTTPS certificate, network availability, and CORS policy for http://localhost:4200.';
    case 401:
      return 'The Billing API rejected the access token (401). Verify the IAM issuer, signing keys, token expiry, and expected audience billing-api.';
    case 403:
      return 'The Billing API denied this operation (403). Verify that the access token has the billing.read scope and that the API authorization policy allows it.';
    case 404:
      return 'The configured Billing API route was not found (404). Verify billing.readEndpoint against the API route definitions.';
    default:
      return `The Billing API returned HTTP ${error.status}. Check the API logs for details.`;
  }
}
