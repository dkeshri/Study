export const environment = {
  production: false,
  iam: {
    // Keep this identical to the `issuer` value returned by the discovery document.
    // Default for the local IAM instance used in this solution.
    issuer: 'https://localhost:7001/',
    clientId: 'angular-web',
    redirectUri: 'http://localhost:4200/auth/callback',
    postLogoutRedirectUri: 'http://localhost:4200/',
    // Request only scopes this SPA needs. Do not request service-to-service scopes.
    scope: 'openid profile email offline_access billing.read'
  },
  billing: {
    // Intentionally blank until the actual Billing API address is supplied.
    // Example format only: 'https://localhost:<billing-api-port>'
    apiBaseUrl: 'https://localhost:7005/',
    // Configure the exact GET route after checking the Billing API controllers.
    // Example format only: 'api/<resource>'
    readEndpoint: 'api/billing/customer/1/charge'
  }
};
