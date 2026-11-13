using Company.IAM.Api.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.IAM.Api.Controllers;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountsController(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request)
    {
        var existingUser = await _userManager
            .FindByEmailAsync(request.Email);

        if (existingUser != null)
        {
            return Conflict(new
            {
                message = "User already exists."
            });
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(
            user,
            request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                errors = result.Errors.Select(x => new
                {
                    x.Code,
                    x.Description
                })
            });
        }

        return CreatedAtAction(
            nameof(GetUser),
            new { id = user.Id },
            new
            {
                user.Id,
                user.Email,
                user.FullName
            });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(new
        {
            user.Id,
            user.Email,
            user.FullName
        });
    }
}

public sealed record RegisterRequest(
    string FullName,
    string Email,
    string Password);