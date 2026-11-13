# Company Portal — Angular + OpenIddict IAM

A standalone Angular SPA starter for the `angular-web` OpenIddict client already present in your IAM database. It provides:

- A branded Login page.
- A Register page that sends the user to IAM's hosted registration UI.
- An OIDC callback route.
- A protected Home page that displays the returned identity claims.
- A Billing API integration boundary that sends the IAM access token as a bearer token and presents loading, error, empty, and success states.
- Authorization Code flow with refresh-token support.
- Session-scoped token storage (`sessionStorage`).

## 1. IAM client values used by this project

This project is configured to match the database record you shared:

| IAM setting | Angular value |
|---|---|
| Client ID | `angular-web` |
| Client type | `public` |
| Client secret | None |
| Grant | Authorization Code |
| Redirect URI | `http://localhost:4200/auth/callback` |
| Post-logout redirect URI | `http://localhost:4200/` |
| Scopes requested | `openid profile email offline_access billing.read` |

The app deliberately does not request `pricing.read` or `users.read`; those are internal service scopes and should not be exposed to the browser client. The app does not contain a client secret.

## 2. Authorization Code + PKCE

The Angular application is a public OAuth client and uses **Authorization Code + PKCE (S256)**. PKCE is explicitly enabled in `src/app/core/auth/auth.config.ts`:

```ts
responseType: 'code',
disablePKCE: false,
```

`angular-oauth2-oidc` generates a cryptographically random `code_verifier`, derives the SHA-256 `code_challenge`, sends the challenge with the `/connect/authorize` request, and sends the matching verifier when redeeming the authorization code at `/connect/token`. The configured `sessionStorage` is used for the browser-side OAuth state, including the pending PKCE flow across the IAM redirect.

Keep the IAM server's global `RequireProofKeyForCodeExchange()` setting enabled. No client secret belongs in Angular. The `angular-web` client can remain a public client with its existing redirect URI and authorization-code permission; the global IAM setting enforces PKCE for it. Postman can continue using its own Authorization Code + PKCE configuration.

When testing, inspect the browser Network tab:

- The `/connect/authorize` request must contain `code_challenge` and `code_challenge_method=S256`.
- The `/connect/token` authorization-code request must contain `code_verifier`.
- Do not copy, log, or manually persist the verifier or access token.

PKCE protects the authorization code against interception/injection; it does not replace HTTPS, strict redirect URI validation, issuer validation, or normal API authorization. Serve the Angular SPA over HTTPS outside local development and prevent XSS.

## 3. Verify the IAM discovery document

Start your IAM API and open:

```text
https://localhost:7001/.well-known/openid-configuration
```

Verify that:

- `issuer` matches `issuer` in `src/environments/environment.ts` exactly (including a trailing slash, if present).
- `authorization_endpoint` is `https://localhost:7001/connect/authorize`.
- `token_endpoint` is `https://localhost:7001/connect/token`.
- `jwks_uri` is present.

If the discovery document reports a different issuer, update `environment.iam.issuer`. Trust the local ASP.NET Core development HTTPS certificate in your browser; otherwise, the browser may block the discovery/token requests.

## 4. Enable CORS on the IAM token/discovery endpoints

The authorization endpoint is reached by a top-level browser redirect, but Angular exchanges the authorization code by making a browser HTTP request to `/connect/token`. The IAM server must allow the SPA origin for discovery and token requests.

Add a narrowly-scoped CORS policy in the IAM `Program.cs` (merge it with any existing CORS configuration):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularSpa", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
```

Place CORS between routing and authentication in the request pipeline:

```csharp
app.UseRouting();
app.UseCors("AngularSpa");
app.UseAuthentication();
app.UseAuthorization();

// Map controllers / Razor Pages after middleware.
```

Do not use `AllowAnyOrigin()` for an IAM server. For production, replace the localhost origin with the exact HTTPS SPA origin.

## 5. Registration flow

The Angular Register page does not collect passwords. It starts the OIDC authorization request and sends the browser to IAM. On the IAM Login page, the user selects the existing **Register** link.

For the authorization request to survive registration, the IAM Identity Login page's Register link must preserve its `returnUrl`, and your Register page must redirect back to Login with the same `returnUrl` after account creation. The custom Register handler should use logic like:

```csharp
if (result.Succeeded)
{
    _logger.LogInformation("User created a new account.");
    return RedirectToPage("./Login", new { returnUrl });
}
```

The Register form must also post its return URL:

```cshtml
<form asp-route-returnUrl="@Model.ReturnUrl" method="post">
```

Then the flow is:

1. Angular starts `/connect/authorize`.
2. IAM displays its Login page.
3. User chooses Register; IAM preserves the OIDC return URL.
4. User registers; IAM returns to Login with that same return URL.
5. User logs in; the Identity cookie authenticates the user.
6. OpenIddict resumes `/connect/authorize` and redirects an authorization code to Angular `/auth/callback`.
7. Angular exchanges the code at `/connect/token` and routes to `/home`.

## 6. Run the Angular project

Requirements: Node.js supported by Angular 21 (Node 20.19+ or 22.12+), and npm.

```bash
npm install
npm start
```

Open:

```text
http://localhost:4200
```

Use the Login button to sign in, or open `/register` to start the registration journey.

Build for deployment:

```bash
npm run build
```

The build output is written to `dist/iam-angular-spa/browser`.

## 7. Configuration locations

- IAM issuer, client ID, callback, scopes: `src/environments/environment.ts`
- OIDC protocol settings including PKCE (S256): `src/app/core/auth/auth.config.ts`
- Authentication lifecycle and token refresh: `src/app/core/auth/auth.service.ts`
- Protected routes: `src/app/core/auth/auth.guard.ts`
- UI pages: `src/app/features/`

## 8. Sign-out note

The Angular Sign out action clears the SPA's token session. A full single-sign-out that also ends the IAM Identity cookie requires OpenIddict's end-session endpoint, the `ept:endsession` permission on `angular-web`, and an IAM logout handler. Your shared client record does not currently show that endpoint permission, so configure it separately if you need full IAM SSO logout.

## 9. Production checklist

- Keep Authorization Code + PKCE (S256) enabled for this public SPA client.
- Serve Angular and IAM only over HTTPS.
- Restrict CORS to the exact SPA origin.
- Use a restrictive Content Security Policy and prevent XSS.
- Keep API authorization on the API; hiding a UI element is not authorization.
- Validate `billing.read` at the Billing API and keep service-to-service scopes on confidential service clients only.
- Configure full OpenID Connect logout if required.


## 10. Billing API integration

The Home page now contains a Billing API panel. The integration is deliberately contract-driven: no Billing API source project, route definitions, response DTO, launch profile, or base URL was present with the Angular project when this integration was prepared. Therefore, the project does **not** invent a route or response schema.

### Configure the real Billing API address and GET route

Edit `src/environments/environment.ts`:

```ts
billing: {
  apiBaseUrl: '',       // Set the Billing API origin from its launchSettings/Docker configuration.
  readEndpoint: ''      // Set an actual GET route from a Billing API controller/minimal API map.
}
```

For example, the format of `apiBaseUrl` is an origin such as `https://localhost:<actual-port>`. The `readEndpoint` is a relative route taken from the real API code, not a guessed endpoint. Do not include a token or secret in this configuration.

When both values are configured, Home makes the GET request on page load and exposes a Refresh action. The generic client method is `BillingApiService.get<TResponse>()`; use the Billing API's actual response DTO as `TResponse` when that contract is available. Until then, the response is treated as `unknown` and rendered as JSON rather than mapping invented fields.

### Authentication behavior

- Uses the access token from the existing `angular-oauth2-oidc` session.
- Adds `Authorization: Bearer <access-token>` only on the Billing API request. It is **not** installed as a global interceptor and is never sent to an arbitrary URL.
- Does not add a client secret or request `pricing.read`/`users.read`; Authorization Code + PKCE (S256) is enabled for `angular-web`.
- The OIDC request already includes `billing.read`; do not add additional service scopes to the SPA.
- A `401` is surfaced as a token/issuer/audience validation issue; a `403` is surfaced as a scope/policy issue. The UI does not print or log the token.

### Billing API authorization expectations

The Billing API must trust the IAM issuer/signing keys, validate the access token audience intended for Billing (in the existing IAM scope/resource setup this is `billing-api`), and enforce `billing.read` on the read endpoint. Do not rely on the Angular UI to enforce authorization. Keep service-to-service scopes such as `pricing.read` and `users.read` on confidential backend clients.

### CORS on the Billing API

CORS on the Billing API is separate from CORS on IAM's discovery/token endpoints. For local development, allow only the Angular origin and only the methods/headers needed by the actual API. For a read-only GET integration, a narrow ASP.NET Core policy can look like this (merge it into the Billing API, do not create a duplicate pipeline):

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularSpa", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .WithMethods("GET")
            .WithHeaders("Authorization", "Accept");
    });
});

// After app.Build():
app.UseRouting();
app.UseCors("AngularSpa");
app.UseAuthentication();
app.UseAuthorization();
// Map the API endpoints after the middleware.
```

A bearer-token SPA does not need cookie-based CORS credentials, so do not add `AllowCredentials()` for this integration. If the actual endpoint uses additional methods or headers, add only those that the API contract requires. In production, replace `http://localhost:4200` with the exact HTTPS Angular origin. Do not use `AllowAnyOrigin()`.

### Exact inputs still needed to finish a domain-specific Billing dashboard

1. The Billing API base URL/port for the environment in which Angular will run.
2. The actual GET route to call (controller route plus action route, or Minimal API route).
3. The response DTO or a representative successful JSON response, including how an empty result is represented.
4. Confirmation that the endpoint is protected by the expected `billing.read` policy and `billing-api` audience.

Once those are known, replace `unknown` with the real DTO and render actual billing concepts (for example, invoices or balances) without guessing field names.

### Billing UI states

- **Not configured:** no request is sent until both config values are supplied.
- **Loading:** shows while the GET request is in flight.
- **Error:** displays a safe message for network/CORS, 401, 403, 404, and other HTTP failures.
- **Empty:** handles `null`, an empty string, an empty array, an empty object, or a no-content response.
- **Success:** shows the received JSON until the actual response model is available.

## 11. Validation status

The Angular source was updated with the typed generic Billing API client, bearer-token handling, environment configuration, Home page states, and setup notes. A TypeScript source syntax parse completed successfully. `npm install --no-audit --no-fund` was attempted but could not reach `registry.npmjs.org` (`EAI_AGAIN` DNS resolution); `node_modules` is therefore absent, so `ng build` and Angular tests could not be run in this environment. No Billing API end-to-end request can be verified until the actual Billing API base URL and route contract are supplied.

## 12. Pricing and Payment pages

The authenticated navigation now includes **Home/Billing**, **Pricing**, and **Payments**. The routes are `/home`, `/pricing`, and `/payment`. Both new pages use typed generic API services and the same safe request lifecycle as Billing: not configured, loading, error, empty, and success. Until the actual endpoint DTO is available, successful responses are displayed as JSON (`unknown`) rather than mapped to guessed business fields.

### Payment API

`payment.read` is included in the Angular OIDC request because the IAM client record you shared already grants `angular-web` the `payment.read` scope permission and your IAM seeder maps that scope to the `payment-api` resource. The Payment API should validate:

- Issuer/signature/lifetime from the IAM authority.
- Audience `payment-api`.
- Scope `payment.read` on read endpoints.

Configure the actual Payment API URL and GET path after inspecting its launch profile and controller/minimal API route:

```ts
payment: {
  apiBaseUrl: '',
  readEndpoint: ''
}
```

### Pricing API: keep the internal service scope private

The existing `pricing.read` scope is the Billing-to-Pricing service-to-service permission in this architecture. Do **not** request that internal scope from Angular or expose the `pricing-service` client credentials in the browser.

The Pricing page is prepared for a separate browser-facing scope named `pricing.catalog.read`, mapped to the `pricing-api` resource. Before enabling direct Angular-to-Pricing calls, register that scope in IAM and grant it to `angular-web`:

1. In the IAM `IdentitySeeder.SeedAsync()`, add:

   ```csharp
   await EnsureScope(scopeManager, "pricing.catalog.read", "pricing-api");
   ```

2. Add `pricing.catalog.read` to the scope permissions for the existing `angular-web` application. Your current `EnsureAngularClient()` returns early when the client already exists, so changing its new-client scope array alone will **not** update the existing database record. Update the existing application descriptor through `IOpenIddictApplicationManager.PopulateAsync()` and `UpdateAsync()`, or apply an equivalent reviewed database migration/administrative update. For example:

   ```csharp
   var application = await appManager.FindByClientIdAsync("angular-web");
   if (application is not null)
   {
       var descriptor = new OpenIddictApplicationDescriptor();
       await appManager.PopulateAsync(descriptor, application);
       descriptor.Permissions.Add(
           OpenIddictConstants.Permissions.Prefixes.Scope + "pricing.catalog.read");
       await appManager.UpdateAsync(application, descriptor);
   }
   ```

3. Add `pricing.catalog.read` to `environment.iam.scope` only after IAM has registered the scope and granted it to `angular-web`:

   ```ts
   scope: 'openid profile email offline_access billing.read payment.read pricing.catalog.read'
   ```

4. The Pricing API must accept the browser-facing `pricing.catalog.read` scope and audience `pricing-api`. Keep the internal `pricing.read` requirement for Billing's confidential service client; do not replace it.

Until those IAM changes are completed, the Pricing page intentionally shows a scope-not-configured message and does not send a request.

### Pricing and Payment API configuration

Edit `src/environments/environment.ts` after checking the actual API source and launch profiles:

```ts
pricing: {
  apiBaseUrl: '',  // actual HTTPS Pricing API origin
  readEndpoint: '' // exact GET route from the API
},
payment: {
  apiBaseUrl: '',  // actual HTTPS Payment API origin
  readEndpoint: '' // exact GET route from the API
}
```

No Pricing or Payment API project, routes, response DTOs, or ports were present in the supplied project files, so these values remain empty by design. Do not copy the sample values from another environment or guess controller routes.

### CORS on Pricing and Payment APIs

CORS is configured separately on each API, just as it is on IAM and Billing. For these read-only bearer-token requests, allow only the Angular origin and the required request headers/method:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularSpa", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .WithMethods("GET")
              .WithHeaders("Authorization", "Accept");
    });
});

// After app.Build():
app.UseRouting();
app.UseCors("AngularSpa");
app.UseAuthentication();
app.UseAuthorization();
```

Apply a matching, narrowly scoped policy to both Pricing and Payment APIs. In production, replace the localhost origin with the exact HTTPS SPA origin. Do not use `AllowAnyOrigin()` or `AllowCredentials()` for this bearer-token flow.

## 13. Shared API client and routes

- `src/app/core/api/authenticated-api-client.service.ts`: validates HTTPS API origin and relative route, attaches the IAM bearer token only to that configured origin, and formats safe API errors.
- `src/app/core/pricing/pricing-api.service.ts`: typed Pricing API GET client.
- `src/app/core/payment/payment-api.service.ts`: typed Payment API GET client.
- `src/app/features/api-resource/api-resource-page.component.ts`: shared authenticated page used by `/pricing` and `/payment`.
- `src/app/app.routes.ts`: protects both pages with the existing auth guard.

The client uses generic `get<TResponse>()` methods. Once the backend contracts are supplied, replace `unknown` with real response DTOs and render domain-specific fields. Do not log access tokens or attach them through a global interceptor.

## 14. Validation status

The source changes were inspected, including route registration, API-service configuration, and page scope gating. A full Angular compiler/build check still requires installing the project dependencies with `npm install`; the npm registry was unreachable in the earlier build environment. No Pricing or Payment end-to-end call can be verified until their real base URLs, routes, and response DTOs are supplied. The Pricing page also requires the dedicated public scope setup above before it can request data.
