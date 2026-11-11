using Company.Payment.Api.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Company.Payment.Api.Controllers;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController(IamUsersClient users) : ControllerBase
{
    [HttpGet("customer/{userId}/profile")]
    [Authorize(Policy = "payment.read")]
    public async Task<IActionResult> GetCustomerProfile(string userId, CancellationToken ct)
    {
        var user = await users.GetUserAsync(userId, ct);
        return user is null ? NotFound() : Ok(user);
    }
}