using AdminPortal.Services;
using AdminPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers;

[Authorize(Roles = "Admin")]
public class BillPayController :Controller
{
    private readonly IApiBillPayService _client;
    public BillPayController(IApiBillPayService client) => _client = client;

    [HttpGet]
    public async Task<IActionResult> Index(bool? isBlocked, CancellationToken ct)
    {
        var items = await _client.GetAllAsync(isBlocked, ct);
        return View(new BillPayListViewModel() { Items = items, FilterIsBlocked = isBlocked });
    }
    
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> ToggleBlock(int id, bool toBlocked, bool? isBlocked, CancellationToken ct)
    {
        var (ok, error) = await _client.SetBlockedAsync(id, toBlocked, ct);
        if (ok) TempData["SuccessMessage"] = toBlocked ? "BillPay blocked." : "BillPay unblocked.";
        else    TempData["ErrorMessage"]   = error ?? "Operation failed.";
        return RedirectToAction(nameof(Index), new { isBlocked });
    }
}