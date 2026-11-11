using Company.IAM.Api.Identity;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Company.IAM.Api.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
        var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await EnsureScope(scopeManager, "billing.read", "billing-api");
        await EnsureScope(scopeManager, "billing.write", "billing-api");
        await EnsureScope(scopeManager, "payment.read", "payment-api");
        await EnsureScope(scopeManager, "payment.write", "payment-api");
        await EnsureScope(scopeManager, "pricing.read", "pricing-api");
        await EnsureScope(scopeManager, "pricing.write", "pricing-api");
        await EnsureScope(scopeManager, "users.read", "iam-api");

        var allUserScopes = new[] { "openid", "profile", "email", "offline_access",
            "billing.read", "billing.write", "payment.read", "payment.write",
            "pricing.read", "pricing.write", "users.read" };

        await EnsureAngularClient(appManager, allUserScopes, configuration);
        await EnsureConfidentialClient(appManager, "billing-service",
            configuration["Clients:Billing:Secret"], "pricing.read");
        await EnsureConfidentialClient(appManager, "payment-service",
            configuration["Clients:Payment:Secret"], "users.read");

        var adminEmail = configuration["BootstrapAdmin:Email"];
        var adminPassword = configuration["BootstrapAdmin:Password"];
        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var user = await userManager.FindByEmailAsync(adminEmail);
            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FullName = configuration["BootstrapAdmin:FullName"] ?? "IAM Administrator",
                    IsActive = true
                };
                var result = await userManager.CreateAsync(user, adminPassword);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Bootstrap user creation failed: " +
                        string.Join("; ", result.Errors.Select(e => e.Description)));
            }
        }
    }

    private static async Task EnsureScope(IOpenIddictScopeManager manager, string name, string resource)
    {
        if (await manager.FindByNameAsync(name) is not null) return;
        var descriptor = new OpenIddictScopeDescriptor { Name = name, DisplayName = name };
        descriptor.Resources.Add(resource);
        await manager.CreateAsync(descriptor);
    }

    private static async Task EnsureAngularClient(
        IOpenIddictApplicationManager manager, string[] scopes, IConfiguration configuration)
    {
        const string clientId = "angular-web";
        if (await manager.FindByClientIdAsync(clientId) is not null) return;

        var redirect = configuration["Clients:Angular:RedirectUri"] ?? "http://localhost:4200/auth/callback";
        var postLogout = configuration["Clients:Angular:PostLogoutRedirectUri"] ?? "http://localhost:4200/";
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = "Angular Web Application",
            ClientType = OpenIddictConstants.ClientTypes.Public,
            ConsentType = OpenIddictConstants.ConsentTypes.Implicit
        };
        descriptor.RedirectUris.Add(new Uri(redirect));
        descriptor.PostLogoutRedirectUris.Add(new Uri(postLogout));
        descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
        descriptor.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
        descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
        descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
        descriptor.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
        foreach (var item in scopes)
            descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + item);
        await manager.CreateAsync(descriptor);
    }

    private static async Task EnsureConfidentialClient(
        IOpenIddictApplicationManager manager, string clientId, string? secret, string allowedScope)
    {
        if (await manager.FindByClientIdAsync(clientId) is not null) return;
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException($"Clients secret for '{clientId}' is missing.");

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            ClientSecret = secret,
            DisplayName = clientId,
            ClientType = OpenIddictConstants.ClientTypes.Confidential,
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                OpenIddictConstants.Permissions.Prefixes.Scope + allowedScope
            }
        };
        await manager.CreateAsync(descriptor);
    }
}