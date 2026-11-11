using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Company.Pricing.Api.Controllers;

[ApiController]
[Route("api/prices")]
public sealed class PricingController : ControllerBase
{
    [HttpGet("customer/{customerId}")]
    [Authorize(Policy = "pricing.read")]
    public IActionResult GetCustomerPrice(string customerId)
    {
        // Replace with the Pricing domain/database query.
        return Ok(1250.50m);
    }
}