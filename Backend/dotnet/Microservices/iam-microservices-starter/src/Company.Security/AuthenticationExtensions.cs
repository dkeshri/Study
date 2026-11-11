using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Company.Security;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddIamAuthentication(
        this IServiceCollection services, IConfiguration configuration,
        params string[] allowedScopes)
    {
        var section = configuration.GetSection("AuthIdentity");
        var issuer = section["IdentityUrl"]
            ?? throw new InvalidOperationException("AuthIdentity:IdentityUrl is missing.");
        var metadata = section["MetadataAddress"]
            ?? throw new InvalidOperationException("AuthIdentity:MetadataAddress is missing.");
        var audience = section["Audience"]
            ?? throw new InvalidOperationException("AuthIdentity:Audience is missing.");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = issuer;
                options.MetadataAddress = metadata;
                options.RequireHttpsMetadata = true;
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        services.AddAuthorization(options =>
        {
            foreach (var scope in allowedScopes.Distinct(StringComparer.Ordinal))
            {
                options.AddPolicy(scope, policy => policy
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => HasScope(context.User, scope)));
            }
        });
        return services;
    }

    public static bool HasScope(ClaimsPrincipal principal, string requiredScope) =>
        principal.FindAll("scope")
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Contains(requiredScope, StringComparer.Ordinal);
}