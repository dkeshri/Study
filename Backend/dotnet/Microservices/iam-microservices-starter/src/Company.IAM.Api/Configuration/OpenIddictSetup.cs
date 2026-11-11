using System.Security.Cryptography.X509Certificates;
using Company.IAM.Api.Data;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;

namespace Company.IAM.Api.Configuration;

public static class OpenIddictSetup
{
    public static IServiceCollection AddIamOpenIddict(
        this IServiceCollection services, IConfiguration configuration)
    {
        var issuer = configuration["OpenIddict:Issuer"]
            ?? throw new InvalidOperationException("OpenIddict:Issuer is required.");

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore()
                    .UseDbContext<ApplicationDbContext>();
            })
            .AddServer(options =>
            {
                options.SetIssuer(new Uri(issuer));
                options.SetAuthorizationEndpointUris("/connect/authorize");
                options.SetTokenEndpointUris("/connect/token");

                // OAuth grants
                options.AllowAuthorizationCodeFlow()
                    .AllowClientCredentialsFlow()
                    .AllowRefreshTokenFlow();

                options.RequireProofKeyForCodeExchange();
                options.SetAccessTokenLifetime(TimeSpan.FromMinutes(5));
                options.SetRefreshTokenLifetime(TimeSpan.FromDays(14));
                options.RegisterScopes(
                    OpenIddictConstants.Scopes.OpenId,
                    OpenIddictConstants.Scopes.Profile,
                    OpenIddictConstants.Scopes.Email,
                    OpenIddictConstants.Scopes.OfflineAccess,
                    "billing.read", "billing.write",
                    "payment.read", "payment.write",
                    "pricing.read", "pricing.write", "users.read");

                // Access tokens are JWTs so resource APIs can validate them locally using JWKS.
                // Authorization codes and refresh tokens remain protected by OpenIddict.
                options.DisableAccessTokenEncryption();

                if (configuration.GetValue<bool>("OpenIddict:UseDevelopmentCertificates"))
                {
                    options.AddDevelopmentSigningCertificate();
                    options.AddDevelopmentEncryptionCertificate();
                }
                else
                {
                    options.AddSigningCertificate(LoadCertificate(configuration,
                        "OpenIddict:SigningCertificatePath", "OpenIddict:SigningCertificatePassword"));
                    options.AddEncryptionCertificate(LoadCertificate(configuration,
                        "OpenIddict:EncryptionCertificatePath", "OpenIddict:EncryptionCertificatePassword"));
                }

                options.UseAspNetCore()
                    .EnableAuthorizationEndpointPassthrough()
                    .EnableTokenEndpointPassthrough();
            });

        return services;
    }

    private static X509Certificate2 LoadCertificate(
        IConfiguration configuration, string pathKey, string passwordKey)
    {
        var path = configuration[pathKey]
            ?? throw new InvalidOperationException($"{pathKey} is required.");
        var password = configuration[passwordKey]
            ?? throw new InvalidOperationException($"{passwordKey} is required.");
        return X509CertificateLoader.LoadPkcs12FromFile(
            path, password, X509KeyStorageFlags.EphemeralKeySet);
    }
}