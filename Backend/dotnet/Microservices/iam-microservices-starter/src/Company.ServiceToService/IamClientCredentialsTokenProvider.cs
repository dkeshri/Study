using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Company.ServiceToService;

public sealed class IamClientCredentialsTokenProvider
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _configuration;
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();

    public IamClientCredentialsTokenProvider(IHttpClientFactory factory, IConfiguration configuration)
    { _factory = factory; _configuration = configuration; }

    public async Task<string> GetTokenAsync(CancellationToken ct)
    {
        var scope = _configuration["ServiceAuth:Scope"]
            ?? throw new InvalidOperationException("ServiceAuth:Scope missing.");
        var key = $"{_configuration["ServiceAuth:ClientId"]}:{scope}";
        if (_cache.TryGetValue(key, out var cached) && cached.ExpiresAt > DateTimeOffset.UtcNow.AddSeconds(15))
            return cached.AccessToken;

        var gate = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
        await gate.WaitAsync(ct);
        try
        {
            if (_cache.TryGetValue(key, out cached) && cached.ExpiresAt > DateTimeOffset.UtcNow.AddSeconds(15))
                return cached.AccessToken;

            var clientId = _configuration["ServiceAuth:ClientId"]
                ?? throw new InvalidOperationException("ServiceAuth:ClientId missing.");
            var secret = _configuration["ServiceAuth:ClientSecret"]
                ?? throw new InvalidOperationException("ServiceAuth:ClientSecret missing.");
            var endpoint = _configuration["ServiceAuth:TokenEndpoint"]
                ?? throw new InvalidOperationException("ServiceAuth:TokenEndpoint missing.");

            var client = _factory.CreateClient("iam-token");
            var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));
            using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", basic);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials", ["scope"] = scope
            });
            using var response = await client.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken: ct)
                ?? throw new InvalidOperationException("IAM returned an empty token response.");
            if (string.IsNullOrWhiteSpace(token.AccessToken) || token.ExpiresIn <= 0)
                throw new InvalidOperationException("IAM returned an invalid token response.");
            var cacheSeconds = Math.Max(1, token.ExpiresIn - 60);
            _cache[key] = new CacheEntry(token.AccessToken, DateTimeOffset.UtcNow.AddSeconds(cacheSeconds));
            return token.AccessToken;
        }
        finally { gate.Release(); }
    }

    private sealed record CacheEntry(string AccessToken, DateTimeOffset ExpiresAt);
    private sealed class TokenResponse
    {
        [JsonPropertyName("access_token")] public string AccessToken { get; set; } = "";
        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
    }
}

public sealed class IamBearerHandler(IamClientCredentialsTokenProvider tokenProvider) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await tokenProvider.GetTokenAsync(ct);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, ct);
    }
}