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
    scope: 'openid profile email offline_access billing.read payment.read pricing.read'
  },
  billing: {
    // Set these only after checking the actual Billing API launch profile and route.
    apiBaseUrl: 'https://localhost:7005/',
    readEndpoint: 'api/billing/customer/1/charge'
  },
  pricing: {
    // Set these only after checking the actual Pricing API launch profile and route.
    apiBaseUrl: 'https://localhost:7003/',
    readEndpoint: 'api/prices/customer/1'
  },
  payment: {
    // Set these only after checking the actual Payment API launch profile and route.
    apiBaseUrl: 'https://localhost:7007/',
    readEndpoint: 'api/payments/customer/507208be-f1e5-46a3-a52d-24dc0b29686c/profile'
  }
};
