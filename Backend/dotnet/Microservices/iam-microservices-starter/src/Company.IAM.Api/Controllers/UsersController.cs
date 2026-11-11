using Company.IAM.Api.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.IAM.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "users.read")]
public sealed class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _users;
    public UsersController(UserManager<ApplicationUser> users) => _users = users;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var user = await _users.FindByIdAsync(id);
        if (user is null || !user.IsActive) return NotFound();
        return Ok(new UserProfile(user.Id, user.FullName, user.Email));
    }

    public sealed record UserProfile(string Id, string? Name, string? Email);
}