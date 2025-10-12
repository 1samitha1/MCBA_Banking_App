using System.Net;
using System.Text.Json;
namespace AdminPortal.Services;

public class ApiAuthClient : IApiAuthClient
{
    private readonly IHttpClientFactory _http;

    public ApiAuthClient(IHttpClientFactory clientFactory) => _http = clientFactory;

    private sealed record LoginResponse(string AccessToken, string Role);

    public async Task<(bool ok, string? token, string? role, DateTimeOffset? expiresAt, string? error)>
        LoginAsync(string username, string password, CancellationToken ct = default)
    {
        var client = _http.CreateClient("AdminApi");
        var res = await client.PostAsJsonAsync("api/Auth/login", new { username, password }, ct);

        if (res.StatusCode == HttpStatusCode.Unauthorized)
            return (false, null, null, null, "Invalid username or password.");
        if (!res.IsSuccessStatusCode)
            return (false, null, null, null, $"API login failed: {(int)res.StatusCode}");

        var payload = await res.Content.ReadFromJsonAsync<LoginResponse>(options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
            cancellationToken: ct);
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
            return (false, null, null, null, "Invalid API response.");

        return (true, payload.AccessToken, payload.Role, null, null); // no expiresAt
    }
}