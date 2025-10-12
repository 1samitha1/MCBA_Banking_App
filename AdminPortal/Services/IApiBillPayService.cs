using AdminPortal.ViewModels;

namespace AdminPortal.Services;

public interface IApiBillPayService
{
    Task<List<BillPayViewModel>> GetAllAsync(bool? isBlocked = null, CancellationToken ct = default);
    Task<(bool ok, string? error)> SetBlockedAsync(int id, bool block, CancellationToken ct = default);
}