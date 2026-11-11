using System.Net.Http.Json;

namespace Company.Billing.Api.Clients;

public sealed class PricingClient(HttpClient httpClient)
{
    public async Task<decimal> GetCustomerChargeAsync(string customerId, CancellationToken ct)
    {
        using var response = await httpClient.GetAsync($"api/prices/customer/{Uri.EscapeDataString(customerId)}", ct);
        response.EnsureSuccessStatusCode();

        var amount = await response.Content.ReadFromJsonAsync<decimal?>(cancellationToken: ct);
        return amount ?? throw new InvalidOperationException("Pricing returned an empty response.");
    }
}