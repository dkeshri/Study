import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, filter } from 'rxjs';
import { OAuthService, OAuthStorage } from 'angular-oauth2-oidc';
import { iamAuthConfig } from './auth.config';

const RETURN_URL_KEY = 'company-portal.return-url';
const DEFAULT_RETURN_URL = '/home';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly oauth = inject(OAuthService);
  private readonly storage = inject(OAuthStorage);
  private readonly authenticatedSubject = new BehaviorSubject<boolean>(false);
  private initialized = false;

  readonly isAuthenticated$ = this.authenticatedSubject.asObservable();
  initializationError: string | null = null;

  constructor() {
    this.oauth.events
      .pipe(filter(event => [
        'token_received',
        'token_refreshed',
        'token_expires',
        'session_terminated',
        'session_error',
        'logout'
      ].includes(event.type)))
      .subscribe(() => this.publishAuthenticationState());
  }

  get isAuthenticated(): boolean {
    return this.oauth.hasValidAccessToken();
  }

  /** Returns the current access token to trusted API clients; never log this value. */
  get accessToken(): string | null {
    const token = this.oauth.getAccessToken();
    return token && token.trim().length > 0 ? token : null;
  }

  async initialize(): Promise<void> {
    if (this.initialized) return;

    this.oauth.configure(iamAuthConfig);

    try {
      await this.oauth.loadDiscoveryDocumentAndTryLogin();
      // Refresh tokens are requested using offline_access. This library refreshes
      // the access token when supported by the IAM token endpoint.
      this.oauth.setupAutomaticSilentRefresh();
      this.initializationError = null;
    } catch (error) {
      console.error('Unable to initialize the IAM OIDC client.', error);
      this.initializationError =
        'The IAM service could not be reached. Verify the IAM URL, HTTPS certificate and CORS configuration.';
    } finally {
      this.initialized = true;
      this.publishAuthenticationState();
    }
  }

  beginLogin(returnUrl: string = DEFAULT_RETURN_URL): void {
    this.storage.setItem(RETURN_URL_KEY, this.normalizeReturnUrl(returnUrl));
    this.oauth.initCodeFlow();
  }

  /**
   * Starts the OIDC authorization request. The IAM Login page should expose its
   * Register link and preserve the incoming ReturnUrl to resume this request.
   */
  beginRegistration(): void {
    this.beginLogin(DEFAULT_RETURN_URL);
  }

  consumeReturnUrl(): string {
    const returnUrl = this.storage.getItem(RETURN_URL_KEY);
    this.storage.removeItem(RETURN_URL_KEY);
    return this.normalizeReturnUrl(returnUrl ?? DEFAULT_RETURN_URL);
  }

  get identityClaims(): Record<string, unknown> {
    const claims = this.oauth.getIdentityClaims();
    return (claims && typeof claims === 'object')
      ? claims as Record<string, unknown>
      : {};
  }

  get displayName(): string {
    const claims = this.identityClaims;
    return this.readClaim(claims, 'name')
      ?? this.readClaim(claims, 'preferred_username')
      ?? this.readClaim(claims, 'email')
      ?? 'Signed-in user';
  }

  get email(): string {
    return this.readClaim(this.identityClaims, 'email') ?? 'Email claim not returned';
  }

  get grantedScopes(): string[] {
    const scope = this.readClaim(this.identityClaims, 'scope');
    return scope ? scope.split(' ') : [];
  }

  logout(): void {
    this.storage.removeItem(RETURN_URL_KEY);
    // Clears the SPA token session. Full IAM SSO logout additionally requires
    // the OpenIddict end-session endpoint and client end-session permission.
    this.oauth.logOut();
    this.publishAuthenticationState();
  }

  private publishAuthenticationState(): void {
    this.authenticatedSubject.next(this.oauth.hasValidAccessToken());
  }

  private normalizeReturnUrl(value: string): string {
    // Only accept same-origin application paths; never redirect to an arbitrary URL.
    if (value.startsWith('/') && !value.startsWith('//')) return value;
    return DEFAULT_RETURN_URL;
  }

  private readClaim(claims: Record<string, unknown>, name: string): string | null {
    const value = claims[name];
    return typeof value === 'string' && value.trim().length > 0 ? value : null;
  }
}
