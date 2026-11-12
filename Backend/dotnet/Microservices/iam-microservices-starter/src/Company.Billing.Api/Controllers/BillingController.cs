using Company.Billing.Api.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Billing.Api.Controllers;

[ApiController]
[Route("api/billing")]
public sealed class BillingController(PricingClient pricing) : ControllerBase
{
    [HttpGet("customer/{customerId}/charge")]
    [Authorize(Policy = "billing.read")]
    public async Task<IActionResult> GetCharge(string customerId, CancellationToken ct)
    {
        var amount = await pricing.GetCustomerChargeAsync(customerId, ct);
        return Ok(new { customerId, amount, currency = "INR" });
    }
}