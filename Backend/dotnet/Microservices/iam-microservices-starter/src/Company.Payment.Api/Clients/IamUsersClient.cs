using System.Net.Http.Json;
namespace Company.Payment.Api.Clients;
public sealed class IamUsersClient(HttpClient httpClient)
{
    public Task<UserProfile?> GetUserAsync(string userId, CancellationToken ct) =>
        httpClient.GetFromJsonAsync<UserProfile>($"api/users/{Uri.EscapeDataString(userId)}", ct);
    public sealed record UserProfile(string Id, string? Name, string? Email);
}