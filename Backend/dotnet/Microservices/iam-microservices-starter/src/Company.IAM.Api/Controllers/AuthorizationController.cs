using Company.IAM.Api.Identity;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using System.Collections.Immutable;
using System.Security.Claims;

namespace Company.IAM.Api.Controllers;

public sealed class AuthorizationController : Controller
{
    private readonly UserManager<ApplicationUser> _users;
    private readonly IOpenIddictScopeManager _scopes;

    public AuthorizationController(UserManager<ApplicationUser> users, IOpenIddictScopeManager scopes)
    {
        _users = users;
        _scopes = scopes;
    }

    [HttpGet("~/connect/authorize")]
    [HttpPost("~/connect/authorize")]
    public async Task<IActionResult> Authorize()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("OpenIddict request is unavailable.");

        var authentication = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
        if (!authentication.Succeeded || authentication.Principal is null)
        {
            var returnUrl = Request.PathBase + Request.Path + QueryString.Create(Request.Query.ToList());
            return Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                },
                IdentityConstants.ApplicationScheme);
        }

        var user = await _users.GetUserAsync(authentication.Principal);
        if (user is null || !user.IsActive)
            return Forbid(IdentityConstants.ApplicationScheme);

        var principal = await CreateUserPrincipalAsync(user, request.GetScopes());
        return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("~/connect/token")]
    [Produces("application/json")]
    public async Task<IActionResult> Exchange()
    {
        var request = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("OpenIddict request is unavailable.");

        if (request.IsClientCredentialsGrantType())
        {
            var identity = new ClaimsIdentity(
                TokenValidationParameters.DefaultAuthenticationType,
                OpenIddictConstants.Claims.Name,
                OpenIddictConstants.Claims.Role);
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, request.ClientId!));
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.ClientId, request.ClientId!));

            var principal = new ClaimsPrincipal(identity);
            principal.SetScopes(request.GetScopes());
            await SetResourcesAsync(principal, principal.GetScopes());
            principal.SetDestinations(_ => new[] { OpenIddictConstants.Destinations.AccessToken });
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        if (request.IsAuthorizationCodeGrantType() || request.IsRefreshTokenGrantType())
        {
            var authentication = await HttpContext.AuthenticateAsync(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            if (!authentication.Succeeded || authentication.Principal is null)
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var subject = authentication.Principal.GetClaim(OpenIddictConstants.Claims.Subject);
            var user = subject is null ? null : await _users.FindByIdAsync(subject);
            if (user is null || !user.IsActive)
                return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            // Rebuild claims from the current user record. Do not blindly trust stale profile/role claims.
            var scopes = authentication.Principal.GetScopes();
            var principal = await CreateUserPrincipalAsync(user, scopes);
            return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return Forbid(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private async Task<ClaimsPrincipal> CreateUserPrincipalAsync(ApplicationUser user, IEnumerable<string> requestedScopes)
    {
        var scopes = requestedScopes.ToHashSet(StringComparer.Ordinal);
        var identity = new ClaimsIdentity(
            TokenValidationParameters.DefaultAuthenticationType,
            OpenIddictConstants.Claims.Name,
            OpenIddictConstants.Claims.Role);

        identity.AddClaim(new Claim(OpenIddictConstants.Claims.Subject, user.Id));
        if (scopes.Contains(OpenIddictConstants.Scopes.Profile) && !string.IsNullOrWhiteSpace(user.FullName))
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Name, user.FullName));
        if (scopes.Contains(OpenIddictConstants.Scopes.Email) && !string.IsNullOrWhiteSpace(user.Email))
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Email, user.Email));

        var roles = await _users.GetRolesAsync(user);
        foreach (var role in roles)
            identity.AddClaim(new Claim(OpenIddictConstants.Claims.Role, role));

        var principal = new ClaimsPrincipal(identity);
        principal.SetScopes(scopes);
        await SetResourcesAsync(principal, scopes);
        principal.SetDestinations(claim =>
        {
            if (claim.Type == OpenIddictConstants.Claims.Subject ||
                claim.Type == OpenIddictConstants.Claims.Name ||
                claim.Type == OpenIddictConstants.Claims.Email ||
                claim.Type == OpenIddictConstants.Claims.Role)
                return new[] { OpenIddictConstants.Destinations.AccessToken,
                    OpenIddictConstants.Destinations.IdentityToken };
            return Array.Empty<string>();
        });
        return principal;
    }

    private async Task SetResourcesAsync(
    ClaimsPrincipal principal,
    IEnumerable<string> scopes)
    {
        // Convert IEnumerable<string> into ImmutableArray<string>.
        var scopeArray = scopes.ToImmutableArray();

        var resources = new List<string>();

        await foreach (var resource in _scopes.ListResourcesAsync(scopeArray))
        {
            resources.Add(resource);
        }

        principal.SetResources(resources);
    }
}