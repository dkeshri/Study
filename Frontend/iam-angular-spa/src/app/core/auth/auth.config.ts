import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../../../environments/environment';

export const iamAuthConfig: AuthConfig = {
  issuer: environment.iam.issuer,
  clientId: environment.iam.clientId,
  redirectUri: environment.iam.redirectUri,
  postLogoutRedirectUri: environment.iam.postLogoutRedirectUri,
  responseType: 'code',
  scope: environment.iam.scope,

  // PKCE is enabled for this public SPA client. angular-oauth2-oidc generates
  // an S256 code_challenge and keeps the matching code_verifier for the token exchange.
  // This satisfies IAM's RequireProofKeyForCodeExchange() enforcement.
  disablePKCE: false,

  // Let the library use the IAM discovery document and validate the issuer/JWKS.
  strictDiscoveryDocumentValidation: true,
  requireHttps: true,
  // Keep protocol/token details out of browser console logs.
  showDebugInformation: false,

  // The browser SPA is a public client. Never add a client secret here.
  useSilentRefresh: false,
  clearHashAfterLogin: true
};
