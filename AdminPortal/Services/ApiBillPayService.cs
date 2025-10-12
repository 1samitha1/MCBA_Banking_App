using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using AdminPortal.ViewModels;
using CustomerPortal.Utility;

namespace AdminPortal.Services;

public class ApiBillPayService : IApiBillPayService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _json;

    public ApiBillPayService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _json = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _json.Converters.Add(new JsonStringEnumConverter()); // handles enum-as-string if API returns it
    }

    private sealed record BillPayDto(
        int BillPayID, int AccountNumber, int PayeeID, string PayeeName,
        decimal Amount, DateTime ScheduleTimeUtc, BillPeriod Period, bool IsBlocked);

    public async Task<List<BillPayViewModel>> GetAllAsync(bool? isBlocked = null, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient("AdminApi");

        // API currently doesn't accept a filter; fetch all:
        var data = await client.GetFromJsonAsync<List<BillPayDto>>("api/BillPay", _json, ct) ?? new();

        // Optional client-side filter to keep your UI behavior
        if (isBlocked is not null)
            data = data.Where(d => d.IsBlocked == isBlocked.Value).ToList();

        return data.Select(d => new BillPayViewModel
        {
            BillPayID = d.BillPayID,
            AccountNumber = d.AccountNumber,
            PayeeName = d.PayeeName,
            Amount = d.Amount,
            ScheduleTimeLocal = d.ScheduleTimeUtc.ToLocalTime(),
            Period = d.Period,
            IsBlocked = d.IsBlocked
        }).ToList();
    }

    public async Task<(bool ok, string? error)> SetBlockedAsync(int id, bool block, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient("AdminApi");

        // POST with JSON body, as your API expects
        var url = $"api/BillPay/block/{id}";
        using var content = JsonContent.Create(new { blocked = block });

        var res = await client.PostAsync(url, content, ct);

        if (res.IsSuccessStatusCode) return (true, null);
        if (res.StatusCode == HttpStatusCode.NotFound) return (false, "BillPay not found.");
        return (false, $"API error: {(int)res.StatusCode} {res.ReasonPhrase}");
    }
}