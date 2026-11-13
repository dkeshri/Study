import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '../auth/auth.service';

export interface ReadApiConfiguration {
  apiBaseUrl: string;
  readEndpoint: string;
}

export interface ApiErrorContext {
  displayName: string;
  requiredScope: string;
  expectedAudience: string;
}

/** Raised before an HTTP request is sent when API configuration is incomplete or unsafe. */
export class ApiConfigurationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'ApiConfigurationError';
  }
}

@Injectable({ providedIn: 'root' })
export class AuthenticatedApiClient {
  private readonly http = inject(HttpClient);
  private readonly auth = inject(AuthService);

  /**
   * Sends a GET request to one explicitly configured HTTPS API origin.
   * The caller supplies the actual response contract as TResponse; use unknown
   * until the backend DTO is available. The token is never attached globally.
   */
  get<TResponse>(
    configuration: ReadApiConfiguration,
    relativeEndpoint: string,
    displayName: string
  ): Observable<TResponse> {
    let url: string;

    try {
      url = this.buildUrl(configuration, relativeEndpoint, displayName);
    } catch (error: unknown) {
      const message = error instanceof Error
        ? error.message
        : `${displayName} API configuration is invalid.`;
      return throwError(() => new ApiConfigurationError(message));
    }

    const accessToken = this.auth.accessToken;
    if (!accessToken) {
      return throwError(() => new ApiConfigurationError(
        `No IAM access token is available. Sign in again before calling the ${displayName} API.`
      ));
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${accessToken}`,
      Accept: 'application/json'
    });

    return this.http.get<TResponse>(url, { headers });
  }

  private buildUrl(
    configuration: ReadApiConfiguration,
    relativeEndpoint: string,
    displayName: string
  ): string {
    const configuredBaseUrl = configuration.apiBaseUrl.trim();
    const endpoint = relativeEndpoint.trim();

    if (!configuredBaseUrl) {
      throw new ApiConfigurationError(
        `Set ${displayName.toLowerCase()}.apiBaseUrl in src/environments/environment.ts to the actual ${displayName} API origin.`
      );
    }

    if (!endpoint) {
      throw new ApiConfigurationError(
        `Set ${displayName.toLowerCase()}.readEndpoint to an actual GET route exposed by the ${displayName} API.`
      );
    }

    // Accept only a relative route, so a caller cannot redirect the bearer token
    // to an arbitrary origin.
    if (/^[a-z][a-z\d+.-]*:/i.test(endpoint) || endpoint.startsWith('//')) {
      throw new ApiConfigurationError(
        `${displayName} API endpoint must be a relative path, not an absolute URL.`
      );
    }

    let base: URL;
    try {
      base = new URL(configuredBaseUrl);
    } catch {
      throw new ApiConfigurationError(
        `${displayName.toLowerCase()}.apiBaseUrl must be a valid absolute URL, such as https://localhost:7002.`
      );
    }

    if (base.username || base.password) {
      throw new ApiConfigurationError(`Do not embed credentials in ${displayName.toLowerCase()}.apiBaseUrl.`);
    }

    if (base.protocol !== 'https:') {
      throw new ApiConfigurationError(
        `${displayName} API must use HTTPS, including local development with a trusted ASP.NET Core development certificate.`
      );
    }

    base.search = '';
    base.hash = '';
    if (!base.pathname.endsWith('/')) base.pathname += '/';

    const resolved = new URL(endpoint.replace(/^\/+/, ''), base);
    if (resolved.origin !== base.origin) {
      throw new ApiConfigurationError(
        `${displayName} API endpoint resolved outside the configured API origin.`
      );
    }

    return resolved.toString();
  }
}

/** Creates API-specific, user-safe messages without exposing response bodies or tokens. */
export function getAuthenticatedApiErrorMessage(
  error: unknown,
  context: ApiErrorContext
): string {
  if (error instanceof ApiConfigurationError) return error.message;

  if (!(error instanceof HttpErrorResponse)) {
    return `An unexpected error occurred while contacting the ${context.displayName} API.`;
  }

  switch (error.status) {
    case 0:
      return `The ${context.displayName} API could not be reached. Check its URL, HTTPS certificate, network availability, and CORS policy for http://localhost:4200.`;
    case 401:
      return `The ${context.displayName} API rejected the access token (401). Verify the IAM issuer, signing keys, token expiry, and expected audience ${context.expectedAudience}.`;
    case 403:
      return `The ${context.displayName} API denied this operation (403). Verify that IAM issued the ${context.requiredScope} scope and that the API authorization policy allows it.`;
    case 404:
      return `The configured ${context.displayName} API route was not found (404). Verify its readEndpoint against the API route definitions.`;
    default:
      return `The ${context.displayName} API returned HTTP ${error.status}. Check the API logs for details.`;
  }
}
