using AdminApi.Dtos;
using CustomerPortal.Models;

namespace AdminApi.Data.Repository;

public interface IBillPayRepository
{
    Task<List<BillPayDto>> GetAllAsync(bool? isBlocked, CancellationToken ct = default);
    Task<BillPay?> GetAsync(int id, CancellationToken ct = default);
    Task SetBlockedAsync(int id, bool block, CancellationToken ct = default);
}