namespace AdminPortal.Services;

public interface IApiAuthClient
{
    Task<(bool ok, string? token, string? role, DateTimeOffset? expiresAt, string? error)>
        LoginAsync(string username, string password, CancellationToken ct = default);
}