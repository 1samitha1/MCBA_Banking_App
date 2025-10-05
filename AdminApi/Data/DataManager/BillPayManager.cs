using AdminApi.Data.Repository;
using CustomerPortal.Data;
using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace AdminApi.Data.DataManager;

public class BillPayManager : IBillPayRepository
{
    private readonly McbaContext _db;

    public BillPayManager(McbaContext db)
    {
        _db = db;
    }

    public async Task<List<BillPay>> GetAllAsync(bool? isBlocked, CancellationToken ct = default)
    {
        return await _db.BillPay.Where(b => b.IsBlocked == isBlocked).ToListAsync(ct);
    }

    public async Task<BillPay?> GetAsync(int id, CancellationToken ct = default)
    {
        return await _db.BillPay.SingleOrDefaultAsync(b=> b.BillPayID == id, ct);
    }

    public async Task SetBlockedAsync(int id, bool block, CancellationToken ct = default)
    {
        var entity = await _db.BillPay.SingleOrDefaultAsync(b => b.BillPayID == id, ct)
                     ?? throw new KeyNotFoundException($"BillPay {id} not found");
        entity.IsBlocked = block;
    }
}