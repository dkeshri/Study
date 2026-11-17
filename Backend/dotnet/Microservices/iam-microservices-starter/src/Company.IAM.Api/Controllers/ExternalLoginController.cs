
using System.Security.Claims;
using Company.IAM.Api.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.IAM.Api.Controllers;

[Route("account/external")]
public sealed class ExternalLoginController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<ExternalLoginController> _logger;

    public ExternalLoginController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ILogger<ExternalLoginController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string provider, string? returnUrl)
    {
        if (!string.Equals(provider, "Google", StringComparison.Ordinal))
            return BadRequest("Unsupported external provider.");

        if (string.IsNullOrWhiteSpace(returnUrl) ||
            !Url.IsLocalUrl(returnUrl))
        {
            return BadRequest("Invalid return URL.");
        }

        var callbackUrl = Url.Action(
            nameof(Callback),
            "ExternalLogin",
            new { returnUrl });

        var properties =
            _signInManager.ConfigureExternalAuthenticationProperties(
                provider,
                callbackUrl!);

        return Challenge(properties, provider);
    }

    [HttpGet("callback")]
    public async Task<IActionResult> Callback(
        string? returnUrl = null,
        string? remoteError = null)
    {
        if (string.IsNullOrWhiteSpace(returnUrl) ||
            !Url.IsLocalUrl(returnUrl))
        {
            return BadRequest("Invalid return URL.");
        }

        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            _logger.LogWarning(
                "Google external authentication failed: {Error}",
                remoteError);

            await ClearExternalCookieAsync();

            TempData["ExternalLoginError"] =
                "Google authentication failed. Please try again.";

            return RedirectToLogin(returnUrl);
        }

        var externalLogin =
            await _signInManager.GetExternalLoginInfoAsync();

        if (externalLogin is null)
        {
            _logger.LogWarning(
                "External login information was not available.");

            await ClearExternalCookieAsync();

            TempData["ExternalLoginError"] =
                "Unable to retrieve Google login information.";

            return RedirectToLogin(returnUrl);
        }

        // 1. Check whether this Google account is already linked.
        var user = await _userManager.FindByLoginAsync(
            externalLogin.LoginProvider,
            externalLogin.ProviderKey);

        if (user is not null)
        {
            if (!user.IsActive)
            {
                await ClearExternalCookieAsync();

                TempData["ExternalLoginError"] =
                    "Your account is disabled. Contact the administrator.";

                return RedirectToLogin(returnUrl);
            }

            await _signInManager.SignInAsync(
                user,
                isPersistent: false);

            await ClearExternalCookieAsync();

            return LocalRedirect(returnUrl);
        }

        // 2. Read Google profile information.
        var email = externalLogin.Principal.FindFirstValue(
            ClaimTypes.Email)
            ?? externalLogin.Principal.FindFirstValue("email");

        var name = externalLogin.Principal.FindFirstValue(
            ClaimTypes.Name)
            ?? externalLogin.Principal.FindFirstValue("name");

        if (string.IsNullOrWhiteSpace(email))
        {
            await ClearExternalCookieAsync();

            TempData["ExternalLoginError"] =
                "Google did not provide an email address.";

            return RedirectToLogin(returnUrl);
        }

        

        // 4. Never automatically link an existing local account
        // using email matching alone.
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            await ClearExternalCookieAsync();

            TempData["ExternalLoginError"] =
                "An account with this email already exists. " +
                "Sign in using your existing account and link Google " +
                "from your account settings.";

            return RedirectToLogin(returnUrl);
        }

        // 5. Create the local IAM user.
        var newUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false,
            FullName = name,
            IsActive = true
        };

        var createResult = await _userManager.CreateAsync(newUser);

        if (!createResult.Succeeded)
        {
            var errors = string.Join(
                "; ",
                createResult.Errors.Select(x => x.Description));

            _logger.LogError(
                "Failed to create IAM user for external login. Errors: {Errors}",
                errors);

            await ClearExternalCookieAsync();

            TempData["ExternalLoginError"] =
                $"Unable to create your IAM account: {errors}";

            return RedirectToLogin(returnUrl);
        }

        // 6. Link the Google identity to the local IAM user.
        var addLoginResult = await _userManager.AddLoginAsync(
            newUser,
            externalLogin);

        if (!addLoginResult.Succeeded)
        {
            var errors = string.Join(
                "; ",
                addLoginResult.Errors.Select(x => x.Description));

            _logger.LogError(
                "Failed to link Google login for IAM user {UserId}. Errors: {Errors}",
                newUser.Id,
                errors);

            // Compensate for user creation if linking fails.
            await _userManager.DeleteAsync(newUser);

            await ClearExternalCookieAsync();

            TempData["ExternalLoginError"] =
                $"Unable to link your Google account: {errors}";

            return RedirectToLogin(returnUrl);
        }

        // 7. Sign in using the local ASP.NET Identity cookie.
        await _signInManager.SignInAsync(
            newUser,
            isPersistent: false);

        await ClearExternalCookieAsync();

        _logger.LogInformation(
            "New IAM user {UserId} registered using Google.",
            newUser.Id);

        // 8. Resume the original OpenIddict authorization request.
        return LocalRedirect(returnUrl);
    }

    private IActionResult RedirectToLogin(string returnUrl)
    {
        return RedirectToPage(
            "/Account/Login",
            new
            {
                area = "Identity",
                returnUrl
            });
    }

    private Task ClearExternalCookieAsync()
    {
        return HttpContext.SignOutAsync(
            IdentityConstants.ExternalScheme);
    }
}
